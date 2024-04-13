using AutoMapper;
using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.Order;
using FooRestaurantApi.DomainLayer.Model.Response;
using FooRestaurantApi.RepositoryLayer.Context;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FooRestaurantApi.RepositoryLayer.RepositoryPattern.Implemantations
{
    public class OrderRepository : Repository<OrderEntity>, IOrderRepository
    {

        public IMapper _mapper { get; }
        PostgreDbContext _dbContext { get; }
        IHttpContextAccessor _httpContextAccessor { get; set; }
        long? _userId { get; set; }

        public OrderRepository(PostgreDbContext dbContext, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.GetUserIdFromHttpContext();
        }

        public async Task<ApiResponseModel<string, long?>> AddOrder(AddOrderRequestModel addOrderRequestModel)
        {
            var apiResponseModel = new ApiResponseModel<string, long?>();
            var invalidFoodIds = new List<long>();
            var now = DateTime.Now;

            var FoodListThatAreEnabled = (await (from Food in _dbContext.Set<FoodEntity>()
                                                 where addOrderRequestModel.FoodAndQuantityList.Select(x => x.FoodId).Contains(Food.Id) && Food.IsEnabled
                                                 select Food).ToListAsync());
            foreach (var FoodAndQuantityRequest in addOrderRequestModel.FoodAndQuantityList)
            {
                if (FoodListThatAreEnabled.Select(x => x.Id).Contains(FoodAndQuantityRequest.FoodId) == false)
                {
                    invalidFoodIds.Add(FoodAndQuantityRequest.FoodId);


                }

            }
            if (invalidFoodIds.Count() > 0)
            {
                apiResponseModel.StatusCode = HttpStatusCode.Conflict;

                apiResponseModel.ErrorData = $"{String.Join(", ", invalidFoodIds)} FoodId's are unavalible for order.";

                return apiResponseModel;


            }

            var userEntity = (await (from user in _dbContext.Set<UserEntity>()
                                     where user.Id == _userId
                                     select user).ToListAsync()).First();

            var orderEntityToInsert = new OrderEntity()
            {
                Address = userEntity.Address,
                CreatedBy = (long)_userId,
                Note = addOrderRequestModel.OrdersNote,
                PaymentMethod = addOrderRequestModel.PaymentMethod,
                UserId = (long)_userId,
                DeliveryDate = addOrderRequestModel.DeliveryDate,
                CreationDate = now
          
            };




            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    _dbContext.Add(orderEntityToInsert);
                    _dbContext.SaveChanges();



                    foreach (var FoodAndQuantityRequest in addOrderRequestModel.FoodAndQuantityList)
                    {
                        var orderItemsEntity = new OrderItemEntity()
                        {
                            CreatedBy = (long)_userId,
                            CreationDate = now,
                            FoodId = FoodAndQuantityRequest.FoodId,
                            Quantity = FoodAndQuantityRequest.Quantity,
                            OrderId = orderEntityToInsert.Id


                        };

                        _dbContext.Add(orderItemsEntity);
                        _dbContext.SaveChanges();
                    }



                    transaction.Commit();
                }
                catch (Exception)
                {
                    _dbContext.ChangeTracker.Clear();
                    throw;

                }
            }

            apiResponseModel.SuccessData = orderEntityToInsert.Id;
            return apiResponseModel;
        }

        public async Task<ApiResponseModel<string, OrderDetailModel>> GetOrderDetail(long id)
        {
            var apiResponseModel = new ApiResponseModel<string, OrderDetailModel>();


            var orderDetailModelList =
          await (from order in _dbContext.Set<OrderEntity>()
                 join user in _dbContext.Set<UserEntity>() on order.UserId equals user.Id
                 join orderItem in _dbContext.Set<OrderItemEntity>() on order.Id equals orderItem.OrderId
                 join Food in _dbContext.Set<FoodEntity>() on orderItem.FoodId equals Food.Id
                 where order.Id == id && user.Id == _userId
                 group new { order, user, orderItem, Food } by new { order.Id, order.CreationDate, order.DeliveryDate, order.Address, order.StatusId, order.PaymentMethod, order.Note, user.UserName } into orderItemGroup

                 select new OrderDetailModel() { Address = orderItemGroup.Key.Address, StatusId = orderItemGroup.Key.StatusId, DeliveryDate = orderItemGroup.Key.DeliveryDate, OrderDate = orderItemGroup.Key.CreationDate, OrderNo = orderItemGroup.Key.Id, TotalPrice = orderItemGroup.Sum(x => x.orderItem.Quantity * x.Food.Price) }).ToListAsync();
            if (orderDetailModelList.Count() == 0)
            {
                apiResponseModel.StatusCode = HttpStatusCode.BadRequest;
                apiResponseModel.ErrorData = $"OrderId {id} does not exist";
                return apiResponseModel;
            }

            var orderDetailModel = orderDetailModelList.First();



            var orderDetailFoodModelList = await (from order in _dbContext.Set<OrderEntity>()
                                                  join orderItem in _dbContext.Set<OrderItemEntity>() on order.Id equals orderItem.OrderId
                                                  join Food in _dbContext.Set<FoodEntity>() on orderItem.FoodId equals Food.Id
                                                  where order.Id == id && order.UserId == _userId
                                                  select new OrderDetailFoodModel() { Image = Food.Image, FoodDescription = Food.Description, FoodName = Food.Name, Quantity = orderItem.Quantity, UnitPrice = Food.Price }).ToListAsync();


            orderDetailModel.OrderDetailFoodList = orderDetailFoodModelList;
            apiResponseModel.SuccessData = orderDetailModel;
            return apiResponseModel;


        }

        public async Task<ApiResponseModel<string, OrderDetailAdminModel>> GetOrderDetailAdmin(long id)
        {
            var apiResponseModel = new ApiResponseModel<string, OrderDetailAdminModel>();


            var orderDetailAdminModelList =
          await (from order in _dbContext.Set<OrderEntity>()
                 join user in _dbContext.Set<UserEntity>() on order.UserId equals user.Id
                 join orderItem in _dbContext.Set<OrderItemEntity>() on order.Id equals orderItem.OrderId
                 join Food in _dbContext.Set<FoodEntity>() on orderItem.FoodId equals Food.Id
                 where order.Id == id
                 group new { order, user, orderItem, Food } by new { order.Id, order.CreationDate, order.DeliveryDate, order.Address, order.StatusId, order.PaymentMethod, order.Note, user.UserName, user.Phone } into orderItemGroup

                 select new OrderDetailAdminModel() { Phone = orderItemGroup.Key.Phone, Address = orderItemGroup.Key.Address, StatusId = orderItemGroup.Key.StatusId, UserName = orderItemGroup.Key.UserName, DeliveryDate = orderItemGroup.Key.DeliveryDate, OrderDate = orderItemGroup.Key.CreationDate, OrderNo = orderItemGroup.Key.Id, TotalPrice = orderItemGroup.Sum(x => x.orderItem.Quantity * x.Food.Price) }).ToListAsync();
            if (orderDetailAdminModelList.Count() == 0)
            {
                apiResponseModel.StatusCode = HttpStatusCode.BadRequest;
                apiResponseModel.ErrorData = $"OrderId {id} does not exist";
                return apiResponseModel;
            }

            var orderDetailAdminModel = orderDetailAdminModelList.First();



            var orderDetailFoodAdminModelList = await (from order in _dbContext.Set<OrderEntity>()
                                                       join orderItem in _dbContext.Set<OrderItemEntity>() on order.Id equals orderItem.OrderId
                                                       join Food in _dbContext.Set<FoodEntity>() on orderItem.FoodId equals Food.Id
                                                       where order.Id == id
                                                       select new OrderDetailFoodAdminModel() { Image = Food.Image, FoodDescription = Food.Description, FoodName = Food.Name, Quantity = orderItem.Quantity, UnitPrice = Food.Price }).ToListAsync();


            orderDetailAdminModel.OrderDetailFoodList = orderDetailFoodAdminModelList;
            apiResponseModel.SuccessData = orderDetailAdminModel;
            return apiResponseModel;
        }

        public async Task<ApiResponseModel<string, IEnumerable<FilteredOrderModel>>> FilterOrder(FilterOrderRequestModel filterOrderModel)
        {

            var pageSize = (filterOrderModel.PageNumber == null || filterOrderModel.PageSize == null) ? int.MaxValue : filterOrderModel.PageSize.Value;
            var pageNumber = (filterOrderModel.PageNumber == null || filterOrderModel.PageSize == null) ? 1 : filterOrderModel.PageNumber.Value;

            var apiResponseModel = new ApiResponseModel<string, IEnumerable<FilteredOrderModel>>();

            var filteredOrderModelList = await (from order in _dbContext.Set<OrderEntity>()
                                                join user in _dbContext.Set<UserEntity>() on order.UserId equals user.Id
                                                join orderItem in _dbContext.Set<OrderItemEntity>() on order.Id equals orderItem.OrderId
                                                join Food in _dbContext.Set<FoodEntity>() on orderItem.FoodId equals Food.Id
                                                where order.UserId == _userId &&

                                                (filterOrderModel.OrderId == null ? true : filterOrderModel.OrderId == order.Id) &&
                                                (filterOrderModel.OrderId == null ? true : filterOrderModel.OrderId == order.Id) &&
                                                (String.IsNullOrEmpty(filterOrderModel.Address) ? true : EF.Functions.Like(order.Address, $"%{filterOrderModel.Address}%")) &&
                                                (filterOrderModel.StatusId == null ? true : filterOrderModel.StatusId == order.StatusId) &&
                                                (filterOrderModel.OrderDateStart == null ? true : filterOrderModel.OrderDateStart <= order.CreationDate) &&
                                              (filterOrderModel.OrderDateEnd == null ? true : filterOrderModel.OrderDateEnd >= order.CreationDate) &&
                                               (filterOrderModel.DeliveryDateStart == null ? true : filterOrderModel.DeliveryDateStart <= order.DeliveryDate) &&
                                                (filterOrderModel.DeliveryDateEnd == null ? true : filterOrderModel.DeliveryDateEnd >= order.DeliveryDate)

                                                group new { order, user, orderItem, Food } by new { order.Id, order.CreationDate, order.DeliveryDate, order.Address, order.StatusId, order.PaymentMethod, order.Note, user.UserName } into orderItemGroup
                                                where (filterOrderModel.PriceStart == null ? true : filterOrderModel.PriceStart <= orderItemGroup.Sum(x => x.orderItem.Quantity * x.Food.Price)) &&
                                                (filterOrderModel.PriceEnd == null ? true : filterOrderModel.PriceEnd >= orderItemGroup.Sum(x => x.orderItem.Quantity * x.Food.Price))

                                                orderby orderItemGroup.Key.Id descending

                                                select new FilteredOrderModel() { Address = orderItemGroup.Key.Address, StatusId = orderItemGroup.Key.StatusId, DeliveryDate = orderItemGroup.Key.DeliveryDate, OrderDate = orderItemGroup.Key.CreationDate, OrderId = orderItemGroup.Key.Id, TotalPrice = orderItemGroup.Sum(x => x.orderItem.Quantity * x.Food.Price) }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            apiResponseModel.SuccessData = filteredOrderModelList;
            return apiResponseModel;
        }

        public async Task<ApiResponseModel<string, IEnumerable<FilteredOrderAdminModel>>> FilterOrderAdmin(FilterOrderRequestAdminModel filterOrderRequestAdminModel)
        {
            var pageSize = (filterOrderRequestAdminModel.PageNumber == null || filterOrderRequestAdminModel.PageSize == null) ? int.MaxValue : filterOrderRequestAdminModel.PageSize.Value;
            var pageNumber = (filterOrderRequestAdminModel.PageNumber == null || filterOrderRequestAdminModel.PageSize == null) ? 1 : filterOrderRequestAdminModel.PageNumber.Value;


            var apiResponseModel = new ApiResponseModel<string, IEnumerable<FilteredOrderAdminModel>>();

            var filteredOrderAdminModelList = await (from order in _dbContext.Set<OrderEntity>()
                                                     join user in _dbContext.Set<UserEntity>() on order.UserId equals user.Id
                                                     join orderItem in _dbContext.Set<OrderItemEntity>() on order.Id equals orderItem.OrderId
                                                     join Food in _dbContext.Set<FoodEntity>() on orderItem.FoodId equals Food.Id
                                                     where
                                                            (filterOrderRequestAdminModel.UserId == null ? true : filterOrderRequestAdminModel.UserId == order.UserId) &&
                                                     (filterOrderRequestAdminModel.OrderId == null ? true : filterOrderRequestAdminModel.OrderId == order.Id) &&
                                                     (filterOrderRequestAdminModel.OrderId == null ? true : filterOrderRequestAdminModel.OrderId == order.Id) &&
                                                     (String.IsNullOrEmpty(filterOrderRequestAdminModel.Address) ? true : EF.Functions.Like(order.Address, $"%{filterOrderRequestAdminModel.Address}%")) &&
                                                     (filterOrderRequestAdminModel.StatusId == null ? true : filterOrderRequestAdminModel.StatusId == order.StatusId) &&
                                                     (filterOrderRequestAdminModel.OrderDateStart == null ? true : filterOrderRequestAdminModel.OrderDateStart <= order.CreationDate) &&
                                                   (filterOrderRequestAdminModel.OrderDateEnd == null ? true : filterOrderRequestAdminModel.OrderDateEnd >= order.CreationDate) &&
                                                    (filterOrderRequestAdminModel.DeliveryDateStart == null ? true : filterOrderRequestAdminModel.DeliveryDateStart <= order.DeliveryDate) &&
                                                     (filterOrderRequestAdminModel.DeliveryDateEnd == null ? true : filterOrderRequestAdminModel.DeliveryDateEnd >= order.DeliveryDate)

                                                     group new { order, user, orderItem, Food } by new { order.Id, order.CreationDate, order.DeliveryDate, order.Address, order.StatusId, order.PaymentMethod, order.Note, user.UserName } into orderItemGroup
                                                     where (filterOrderRequestAdminModel.PriceStart == null ? true : filterOrderRequestAdminModel.PriceStart <= orderItemGroup.Sum(x => x.orderItem.Quantity * x.Food.Price)) &&
                                                     (filterOrderRequestAdminModel.PriceEnd == null ? true : filterOrderRequestAdminModel.PriceEnd >= orderItemGroup.Sum(x => x.orderItem.Quantity * x.Food.Price))


                                                     orderby orderItemGroup.Key.Id descending
                                                     select new FilteredOrderAdminModel() { UserName = orderItemGroup.Key.UserName, Address = orderItemGroup.Key.Address, StatusId = orderItemGroup.Key.StatusId, DeliveryDate = orderItemGroup.Key.DeliveryDate, OrderDate = orderItemGroup.Key.CreationDate, OrderId = orderItemGroup.Key.Id, TotalPrice = orderItemGroup.Sum(x => x.orderItem.Quantity * x.Food.Price) }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            apiResponseModel.SuccessData = filteredOrderAdminModelList;
            return apiResponseModel;
        }
    }
}
