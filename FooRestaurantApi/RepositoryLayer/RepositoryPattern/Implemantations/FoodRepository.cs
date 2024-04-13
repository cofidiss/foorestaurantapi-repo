using AutoMapper;
using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.Food;
using FooRestaurantApi.DomainLayer.Model.Response;
using FooRestaurantApi.RepositoryLayer.Context;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FooRestaurantApi.RepositoryLayer.RepositoryPattern.Implemantations
{
    public class FoodRepository : Repository<FoodEntity>, IFoodRepository
    {
        public IMapper _mapper { get; }
        PostgreDbContext _dbContext { get; }
        IHttpContextAccessor _httpContextAccessor { get; set; }
        long? _userId { get; set; }
        public FoodRepository(PostgreDbContext dbContext, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

            _userId = _httpContextAccessor.GetUserIdFromHttpContext();

        }

        public async Task<ApiResponseModel<string, long>> CreateFood(CreateFoodRequestModel createFoodRequestModel)
        {


            var apiResponseModel = new ApiResponseModel<string, long>();


            var FoodListInDb = await Filter(x => x.IsEnabled && x.CategoryId == createFoodRequestModel.CategoryId);
            var now = DateTime.Now;
            foreach (var FoodEntity in FoodListInDb)
            {
                if (FoodEntity.OrderInCategory >= createFoodRequestModel.OrderInCategory)
                {
                    FoodEntity.OrderInCategory = FoodEntity.OrderInCategory + 1;
                    FoodEntity.LastUpdatedBy = _userId;
                    FoodEntity.LastUpdateDate = now;
                }


            }
            var FoodEnttitytoInsert = _mapper.Map<CreateFoodRequestModel, FoodEntity>(createFoodRequestModel);


            FoodEnttitytoInsert.CreationDate = now;
            FoodEnttitytoInsert.CreatedBy = (long)_userId;
            _dbContext.UpdateRange(FoodListInDb);
            _dbContext.Add(FoodEnttitytoInsert);
            await _dbContext.SaveChangesAsync();
            apiResponseModel.SuccessData = FoodEnttitytoInsert.Id;
            return apiResponseModel;
        }

        public async Task<ApiResponseModel<string, long>> UpdateFood(UpdateFoodRequestModel updateFoodRequestModel)
        {
            var apiResponseModel = new ApiResponseModel<string, long>();

            var now = DateTime.Now;
            var entityToUpdate = (await (from Food in _dbContext.Set<FoodEntity>()
                                         where Food.Id == updateFoodRequestModel.Id && Food.IsEnabled
                                         select Food).ToListAsync()).First();


            var sourceCategeryEnabledFoodList = (await (from Food in _dbContext.Set<FoodEntity>()
                                                        where Food.CategoryId == entityToUpdate.CategoryId && Food.IsEnabled == true &&
                                                        Food.OrderInCategory > entityToUpdate.OrderInCategory
                                                        select Food).ToListAsync());


            foreach (var sourceCategeryEnabledFood in sourceCategeryEnabledFoodList)
            {

                sourceCategeryEnabledFood.OrderInCategory = sourceCategeryEnabledFood.OrderInCategory - 1;
                sourceCategeryEnabledFood.LastUpdateDate = now;
                sourceCategeryEnabledFood.LastUpdatedBy = _userId;



            }
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                try
                {



                    _dbContext.UpdateRange(sourceCategeryEnabledFoodList);
                    await _dbContext.SaveChangesAsync();

                    var destCategeryEnabledFoodList = (await (from Food in _dbContext.Set<FoodEntity>()
                                                              where Food.CategoryId == updateFoodRequestModel.CategoryId && Food.IsEnabled == true &&
                                                                           Food.OrderInCategory >= updateFoodRequestModel.OrderInCategory && Food.Id != entityToUpdate.Id
                                                              select Food).ToListAsync());
                    foreach (var destCategeryEnabledFood in destCategeryEnabledFoodList)
                    {

                        destCategeryEnabledFood.OrderInCategory = destCategeryEnabledFood.OrderInCategory + 1;
                        destCategeryEnabledFood.LastUpdateDate = now;
                        destCategeryEnabledFood.LastUpdatedBy = _userId;



                    }

                    _dbContext.UpdateRange(destCategeryEnabledFoodList);
                    await _dbContext.SaveChangesAsync();


                    var entityToInsert = _mapper.Map<UpdateFoodRequestModel, FoodEntity>(updateFoodRequestModel);

                    entityToInsert.CreationDate = now;
                    entityToInsert.CreatedBy = (long)_userId;

                    entityToUpdate.IsEnabled = false;
                    entityToUpdate.LastUpdateDate = now;
                    entityToUpdate.LastUpdatedBy = _userId;

                    _dbContext.Update(entityToUpdate);
                    _dbContext.Add(entityToInsert);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    apiResponseModel.SuccessData = entityToInsert.Id;

                }
                catch (Exception)
                {

                    _dbContext.ChangeTracker.Clear();
                    throw;
                }

            }









            return apiResponseModel;

        }

        public async Task<ApiResponseModel<string, string>> DeleteFood(long id)
        {

            var apiResponseModel = new ApiResponseModel<string, string>() { SuccessData = String.Empty };
            var now = DateTime.Now;
            var entityToSoftDelete = (await (from Food in _dbContext.Set<FoodEntity>()
                                             where Food.Id == id && Food.IsEnabled
                                             select Food).ToListAsync()).First();
            var FoodsInThisCategoryList = (await (from Food in _dbContext.Set<FoodEntity>()
                                                  where Food.CategoryId == entityToSoftDelete.CategoryId && Food.IsEnabled == true
                                                  select Food).ToListAsync());
            foreach (var FoodInThisCategory in FoodsInThisCategoryList)
            {
                if (FoodInThisCategory.OrderInCategory > entityToSoftDelete.OrderInCategory)
                {
                    FoodInThisCategory.OrderInCategory = FoodInThisCategory.OrderInCategory - 1;
                    FoodInThisCategory.LastUpdatedBy = _userId;
                    FoodInThisCategory.LastUpdateDate = now;

                }
            }
            entityToSoftDelete.LastUpdatedBy = _userId;
            entityToSoftDelete.LastUpdateDate = now;
            entityToSoftDelete.IsEnabled = false;
            _dbContext.UpdateRange(FoodsInThisCategoryList);
            _dbContext.Update(entityToSoftDelete);
            await _dbContext.SaveChangesAsync();
            return apiResponseModel;
        }

        public async Task<ApiResponseModel<string, IEnumerable<FoodsForTheCategoryAdminModel>>> GetFoodByWeekDayAdmin(int id)
        {
            var apiResponseModel = new ApiResponseModel<string, IEnumerable<FoodsForTheCategoryAdminModel>>();

            var FoodsForTheCategoryModelList = new List<FoodsForTheCategoryAdminModel>();
            var FoodAndCategoryForAdminModelList = await (from foodCategory in _dbContext.Set<FoodCategoryEntity>()
                                                          join food in _dbContext.Set<FoodEntity>()
                                                               on new { Id = foodCategory.Id, IsEnabled = true }
                                                               equals new { Id = food.CategoryId, IsEnabled = food.IsEnabled } into groupedFood
                                                          from groupedFoodCategory in groupedFood.DefaultIfEmpty()
                                                          where foodCategory.DayId == id
                                                          select new FoodAndCategoryModel(groupedFoodCategory, foodCategory))
                              .ToListAsync();

            var FoodAndCategoryGroupedByCategory = FoodAndCategoryForAdminModelList.GroupBy(p => p.CategoryId);
            foreach (var group in FoodAndCategoryGroupedByCategory)
            {
                var FoodForTheUserModelList = new List<FoodInfoAdminModel>();
                var FoodsForTheCategoryModel = new FoodsForTheCategoryAdminModel()
                {
                    CategoryName = FoodAndCategoryForAdminModelList.Where(x => x.CategoryId == group.Key).ToList().First().CategoryName,
                    FoodList = FoodForTheUserModelList,
                    InDayOrder = group.Select(x => x.OrderInDay).First(),CategoryId=group.Key
                };
                FoodsForTheCategoryModelList.Add(FoodsForTheCategoryModel);
                foreach (var FoodAndCategoryForUserModel in group)
                {
                    if (FoodAndCategoryForUserModel.FoodId == null)
                    {
                        continue;
                    }

                    var FoodForTheUserModel = new FoodInfoAdminModel()
                    {
                        Description = FoodAndCategoryForUserModel.FoodDescription,
                        Image = FoodAndCategoryForUserModel.Image,
                        Id = FoodAndCategoryForUserModel.FoodId.Value,
                        Name = FoodAndCategoryForUserModel.FoodName,
                        OrderInCategory = FoodAndCategoryForUserModel.OrderInCategory.Value,
                        Price = FoodAndCategoryForUserModel.Price.Value


                    };
                    FoodForTheUserModelList.Add(FoodForTheUserModel);


                }
                FoodsForTheCategoryModel.FoodList = FoodsForTheCategoryModel.FoodList.OrderBy(x => x.OrderInCategory).ToList();
            }
            FoodsForTheCategoryModelList = FoodsForTheCategoryModelList.OrderBy(x => x.InDayOrder).ToList();
            apiResponseModel.SuccessData = FoodsForTheCategoryModelList;
            return apiResponseModel;
        }

        public async Task<ApiResponseModel<string, IEnumerable<FoodsForTheCategoryModel>>> GetFoodByWeekDay(int id)
        {
            var apiResponseModel = new ApiResponseModel<string, IEnumerable<FoodsForTheCategoryModel>>();
            var dayofTheWeek = DateTime.Now.DayOfWeek;
            var FoodsForTheCategoryModelList = new List<FoodsForTheCategoryModel>();
            var FoodAndCategoryModelList = await (from foodCategory in _dbContext.Set<FoodCategoryEntity>()
                                                  join food in _dbContext.Set<FoodEntity>()
                                                       on new { Id = foodCategory.Id, IsEnabled = true }
                                                       equals new { Id = food.CategoryId, IsEnabled = food.IsEnabled } into groupedFood
                                                  from groupedFoodCategory in groupedFood.DefaultIfEmpty()
                                                  where foodCategory.DayId == id
                                                  select new FoodAndCategoryModel(groupedFoodCategory, foodCategory))
                              .ToListAsync();

            var FoodAndCategoryGroupedByCategory = FoodAndCategoryModelList.GroupBy(p => p.CategoryId);
            foreach (var group in FoodAndCategoryGroupedByCategory)
            {
                var FoodForTheUserModelList = new List<FoodInfoModel>();
                var FoodsForTheCategoryModel = new FoodsForTheCategoryModel()
                {
                    CategoryName = FoodAndCategoryModelList.Where(x => x.CategoryId == group.Key).ToList().First().CategoryName,
                    FoodList = FoodForTheUserModelList,
                    InDayOrder = group.Select(x => x.OrderInDay).First()
                };
                FoodsForTheCategoryModelList.Add(FoodsForTheCategoryModel);
                foreach (var FoodAndCategoryForUserModel in group)
                {
                    if (FoodAndCategoryForUserModel.FoodId == null)
                    {
                        continue;
                    }

                    var FoodForTheUserModel = new FoodInfoModel()
                    {
                        Description = FoodAndCategoryForUserModel.FoodDescription,
                        Image = FoodAndCategoryForUserModel.Image,
                        Id = FoodAndCategoryForUserModel.FoodId.Value,
                        Name = FoodAndCategoryForUserModel.FoodName,
                        OrderInCategory = FoodAndCategoryForUserModel.OrderInCategory.Value,
                        Price = FoodAndCategoryForUserModel.Price.Value,
                        IsOrderingEnabled = (FoodAndCategoryForUserModel.DayId == (int)dayofTheWeek)




                    };
                    FoodForTheUserModelList.Add(FoodForTheUserModel);


                }
                FoodsForTheCategoryModel.FoodList = FoodsForTheCategoryModel.FoodList.OrderBy(x => x.OrderInCategory).ToList();
            }
            FoodsForTheCategoryModelList = FoodsForTheCategoryModelList.OrderBy(x => x.InDayOrder).ToList();
            apiResponseModel.SuccessData = FoodsForTheCategoryModelList;
            return apiResponseModel;
        }


    }
}
