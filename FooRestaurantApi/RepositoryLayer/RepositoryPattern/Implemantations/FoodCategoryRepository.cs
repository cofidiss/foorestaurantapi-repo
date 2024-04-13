using AutoMapper;
using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.FoodCategory;
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
    public class FoodCategoryRepository : Repository<FoodCategoryEntity>, IFoodCategoryRepository
    {
        PostgreDbContext _dbContext { get; }
        IHttpContextAccessor _httpContextAccessor { get; set; }

        long? _userId { get; set; }
        public IMapper _mapper { get; }
        public FoodCategoryRepository(PostgreDbContext dbContext, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;

            _userId = _httpContextAccessor.GetUserIdFromHttpContext();
            _mapper = mapper;

        }

        public async Task<ApiResponseModel<string, long>> CreateFoodCategory(CreateFoodCategoryRequestModel createFoodCategoryRequestModel)
        {

            var apiResponseModel = new ApiResponseModel<string, long>();
            var categoryListInDb = await Filter(x => x.DayId == createFoodCategoryRequestModel.DayId);
            var now = DateTime.Now;
            foreach (var categoryEntity in categoryListInDb)
            {
                if (categoryEntity.OrderInDay >= createFoodCategoryRequestModel.OrderInDay)
                {
                    categoryEntity.OrderInDay = categoryEntity.OrderInDay + 1;
                    categoryEntity.LastUpdatedBy = _userId;
                    categoryEntity.LastUpdateDate = now;
                }


            }
            var categoryEnttitytoInsert = _mapper.Map<CreateFoodCategoryRequestModel, FoodCategoryEntity>(createFoodCategoryRequestModel);


            categoryEnttitytoInsert.CreationDate = now;
            categoryEnttitytoInsert.CreatedBy = (long)_userId;
            _dbContext.UpdateRange(categoryListInDb);
            _dbContext.Add(categoryEnttitytoInsert);
            await _dbContext.SaveChangesAsync();
            apiResponseModel.SuccessData = categoryEnttitytoInsert.Id;
            return apiResponseModel;
        }

        public async Task<ApiResponseModel<string, long>> UpdateFoodCategory(UpdateFoodCategoryRequestModel updateFoodCategoryRequestModel)
        {
            var apiResponseModel = new ApiResponseModel<string, long>();

            var now = DateTime.Now;
            var entityToUpdate = (await (from FoodCategory in _dbContext.Set<FoodCategoryEntity>()
                                         where FoodCategory.Id == updateFoodCategoryRequestModel.Id
                                         select FoodCategory).ToListAsync()).First();


            var sourceDayCategoryList = (await (from FoodCategory in _dbContext.Set<FoodCategoryEntity>()
                                                where FoodCategory.DayId == entityToUpdate.DayId && FoodCategory.OrderInDay > entityToUpdate.OrderInDay
                                                select FoodCategory).ToListAsync());






            foreach (var sourceDayCategory in sourceDayCategoryList)
            {


                sourceDayCategory.OrderInDay = sourceDayCategory.OrderInDay - 1;
                sourceDayCategory.LastUpdateDate = now;
                sourceDayCategory.LastUpdatedBy = _userId;

            }


            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                try
                {

                    _dbContext.UpdateRange(sourceDayCategoryList);
                    await _dbContext.SaveChangesAsync();






                    var destDayCategoryList = (await (from FoodCategory in _dbContext.Set<FoodCategoryEntity>()
                                                      where FoodCategory.DayId == updateFoodCategoryRequestModel.DayId && FoodCategory.OrderInDay >= updateFoodCategoryRequestModel.OrderInDay
                                                      && FoodCategory.Id != updateFoodCategoryRequestModel.Id
                                                      select FoodCategory).ToListAsync());
                    foreach (var destDayCategory in destDayCategoryList)
                    {


                        destDayCategory.OrderInDay = destDayCategory.OrderInDay + 1;
                        destDayCategory.LastUpdateDate = now;
                        destDayCategory.LastUpdatedBy = _userId;

                    }

                    _dbContext.UpdateRange(destDayCategoryList);
                    await _dbContext.SaveChangesAsync();



                    entityToUpdate.OrderInDay = updateFoodCategoryRequestModel.OrderInDay;
                    entityToUpdate.DayId = updateFoodCategoryRequestModel.DayId;
                    entityToUpdate.Name = updateFoodCategoryRequestModel.CategoryName;
                    entityToUpdate.LastUpdateDate = now;
                    entityToUpdate.LastUpdatedBy = _userId;

                    _dbContext.UpdateRange(sourceDayCategoryList);
                    _dbContext.UpdateRange(destDayCategoryList);
                    _dbContext.Update(entityToUpdate);

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    apiResponseModel.SuccessData = entityToUpdate.Id;


                }

                catch (Exception)
                {
                    _dbContext.ChangeTracker.Clear();
                    throw;


                }

            }




            return apiResponseModel;

        }

        public async Task<ApiResponseModel<string, string>> DeleteFoodCategory(long id)
        {
            var apiResponseModel = new ApiResponseModel<string, string>() { SuccessData = String.Empty };
            var now = DateTime.Now;
            var FoodCategoryEntityList = (await (from FoodCategory in _dbContext.Set<FoodCategoryEntity>()
                                                 select FoodCategory).ToListAsync());
            var foodCategoryEntityToDelete = FoodCategoryEntityList.Where(x => x.Id == id).First();

            var FoodListToSoftDeleteForCategory = (await (from Food in _dbContext.Set<FoodEntity>()
                                                          where Food.CategoryId == id && Food.IsEnabled == true
                                                          select Food).ToListAsync());

            foreach (var FoodToSoftDeleteForCategory in FoodListToSoftDeleteForCategory)
            {
                FoodToSoftDeleteForCategory.IsEnabled = false;
                FoodToSoftDeleteForCategory.LastUpdateDate = now;
                FoodToSoftDeleteForCategory.LastUpdatedBy = _userId;



            }

            foreach (var foodCategoryEntity in FoodCategoryEntityList)
            {
                if (foodCategoryEntityToDelete.OrderInDay < foodCategoryEntity.OrderInDay)
                {


                    foodCategoryEntity.OrderInDay = foodCategoryEntity.OrderInDay - 1; ;
                    foodCategoryEntity.LastUpdateDate = now;
                    foodCategoryEntity.LastUpdatedBy = _userId;


                }



            }




            _dbContext.UpdateRange(FoodListToSoftDeleteForCategory);
            _dbContext.UpdateRange(FoodCategoryEntityList);
            _dbContext.Remove(foodCategoryEntityToDelete);
            await _dbContext.SaveChangesAsync();
            return apiResponseModel;
        }

        public async Task<ApiResponseModel<string, IEnumerable<FoodCategoryLovAdminModel>>> GetFoodCategoryLovAdmin()
        {
            var apiResponseModel = new ApiResponseModel<string, IEnumerable<FoodCategoryLovAdminModel>>();

            var now = DateTime.Now;
            var categorylovlist = (await (from FoodCategory in _dbContext.Set<FoodCategoryEntity>()

                                          select new FoodCategoryLovAdminModel() { Id = FoodCategory.Id, Name = FoodCategory.Name, DayId = FoodCategory.DayId }).ToListAsync());

            apiResponseModel.SuccessData = categorylovlist;
            return apiResponseModel;
        }
    }
}
