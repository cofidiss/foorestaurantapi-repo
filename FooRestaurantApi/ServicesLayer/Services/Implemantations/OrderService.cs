using AutoMapper;
using FooRestaurantApi.DomainLayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.Enums;
using FooRestaurantApi.DomainLayer.Model.Order;
using FooRestaurantApi.DomainLayer.Model.Response;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;

using FooRestaurantApi.ServicesLayer.DTO.Order;
using FooRestaurantApi.ServicesLayer.DTO.Response;
using FooRestaurantApi.ServicesLayer.DTO.User;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FooRestaurantApi.ServicesLayer.Services.Implemantations
{
    public class OrderService : IOrderService
    {
        public IMapper _mapper { get; }
        public IOrderRepository _orderRepository { get; }
        IHttpContextAccessor _httpContextAccessor { get; }
        string _cartSessionKey { get; set; }
        long? _userId;
        IConfiguration _configuration { get; }
        ICartRepository _cartRepository { get; }
        public OrderService(IMapper mapper, IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ICartRepository cartRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.GetUserIdFromHttpContext();

            _configuration = configuration;
            _cartSessionKey = _configuration.GetSection("cartSessionKey").Value;
            _cartRepository = cartRepository;
        }

        public async Task<ApiResponseDto<string, long?>> AddOrder(AddOrderRequestDto addOrderRequestDto)
        {
            var addOrderRequestModel = _mapper.Map<AddOrderRequestDto, AddOrderRequestModel>(addOrderRequestDto);

            var session = _httpContextAccessor.HttpContext.Session;
            if (!session.Keys.Contains(_cartSessionKey))
            {
                return new ApiResponseDto<string, long?>() { ErrorData = "Cart is Empty. Please add items.", StatusCode = HttpStatusCode.BadRequest };


            }
            else
            {

                var cartItemsString = session.GetString(_cartSessionKey);
                var cartItemDtoList = JsonSerializer.Deserialize<IList<CartItemDto>>(cartItemsString);
                if (cartItemDtoList.Count() == 0)
                {

                    return new ApiResponseDto<string, long?>() { ErrorData = "Cart is Empty. Please add items.", StatusCode = HttpStatusCode.BadRequest };

                }
                var avalibleFoodEntityiesForToday = await _cartRepository.GetAvalibleFoodEntityiesForToday(cartItemDtoList.Select(x => x.FoodId));

                var updatedCartItemDtoList = cartItemDtoList.Where(x => avalibleFoodEntityiesForToday.Select(x => x.Id).Contains(x.FoodId));

                if (updatedCartItemDtoList.Count() != cartItemDtoList.Count())
                {
                    session.SetString(_cartSessionKey, JsonSerializer.Serialize(new List<CartItemDto>()));
                    return new ApiResponseDto<string, long?>() { ErrorData = "Some items are unavalible for order. Cart is emptied.", StatusCode = HttpStatusCode.BadRequest };



                }


                addOrderRequestModel.FoodAndQuantityList = _mapper.Map<IEnumerable<CartItemDto>, IEnumerable<FoodAndQuantityForOrderModel>>(cartItemDtoList);

            }






            var apiResponseModel = await _orderRepository.AddOrder(addOrderRequestModel);
            session.SetString(_cartSessionKey, JsonSerializer.Serialize(new List<CartItemDto>()));
            return _mapper.Map<ApiResponseModel<string, long?>, ApiResponseDto<string, long?>>(apiResponseModel);



        }

        public async Task<ApiResponseDto<string, OrderDetailDto>> GetOrderDetail(long id)
        {

            var apiResponseModel = await _orderRepository.GetOrderDetail(id);
            return _mapper.Map<ApiResponseModel<string, OrderDetailModel>, ApiResponseDto<string, OrderDetailDto>>(apiResponseModel);
        }

        public async Task<ApiResponseDto<string, OrderDetailAdminDto>> GetOrderDetailAdmin(long id)
        {
            var apiResponseModel = await _orderRepository.GetOrderDetailAdmin(id);
            return _mapper.Map<ApiResponseModel<string, OrderDetailAdminModel>, ApiResponseDto<string, OrderDetailAdminDto>>(apiResponseModel);
        }

        public Task UpdateOrderStatusAdmin(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponseDto<string, bool>> IsOrderStatusUpdateValidForUser(long orderId, int statusId)
        {

            var apiResponseDto = new ApiResponseDto<string, bool>();
            var orderList = await _orderRepository.Filter(x => x.Id == orderId);

            if (orderList.Count() == 0)
            {

                apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
                apiResponseDto.ErrorData = $"OrderId {orderId} is invalid";
                return apiResponseDto;
            }
            else
            {
                var previousStatusId = orderList.First().StatusId;
                if ((int)OrderStatusEnum.Received == previousStatusId)
                {

                    var validStatusList = new List<OrderStatusEnum>() { OrderStatusEnum.Cancelled


                    };
                    if (!validStatusList.Select(x => (int)x).Contains(statusId))
                    {

                        apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
                        apiResponseDto.ErrorData = $"OrderStatusId {statusId} is invalid";
                        return apiResponseDto;
                    }

                }


                else if ((int)OrderStatusEnum.Confirmed == previousStatusId)
                {

                    var validStatusList = new List<OrderStatusEnum>() { OrderStatusEnum.Cancelled


                    };
                    if (!validStatusList.Select(x => (int)x).Contains(statusId))
                    {

                        apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
                        apiResponseDto.ErrorData = $"OrderStatusId {statusId} is invalid";
                        return apiResponseDto;
                    }

                }
                else
                {

                    apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
                    apiResponseDto.ErrorData = $"OrderStatusId {statusId} is invalid";
                    return apiResponseDto;

                }




            }
            apiResponseDto.SuccessData = true;
            return apiResponseDto;
        }

        public async Task<ApiResponseDto<string, long>> UpdateOrderStatus(long orderId, int statusId)
        {
            var apiResponseDto = new ApiResponseDto<string, long>();
            var orderList = await _orderRepository.Filter(x => x.Id == orderId);
            var orderEntityToUpdate = orderList.First();
            orderEntityToUpdate.StatusId = statusId;
            orderEntityToUpdate.LastUpdateDate = DateTime.Now;
            orderEntityToUpdate.LastUpdatedBy = _userId;

            await _orderRepository.Update(orderEntityToUpdate);

            apiResponseDto.SuccessData = orderEntityToUpdate.Id;

            return apiResponseDto;


        }

        public async Task<ApiResponseDto<string, bool>> IsOrderStatusUpdateValidForAdmin(long orderId, int statusId)
        {
            var apiResponseDto = new ApiResponseDto<string, bool>();
            var orderList = await _orderRepository.Filter(x => x.Id == orderId);

            if (orderList.Count() == 0)
            {

                apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
                apiResponseDto.ErrorData = $"OrderId {orderId} is does not exists.";
                return apiResponseDto;
            }
            else
            {
                var previousStatusId = orderList.First().StatusId;
                if ((int)OrderStatusEnum.Received == previousStatusId)
                {

                    var validStatusList = new List<OrderStatusEnum>() { OrderStatusEnum.Cancelled,
                        OrderStatusEnum.Confirmed


                    };
                    if (!validStatusList.Select(x => (int)x).Contains(statusId))
                    {

                        apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
                        apiResponseDto.ErrorData = $"OrderStatusId {statusId} is invalid";
                        return apiResponseDto;
                    }

                }


                else if ((int)OrderStatusEnum.Confirmed == previousStatusId)
                {

                    var validStatusList = new List<OrderStatusEnum>() { OrderStatusEnum.Cancelled,OrderStatusEnum.Preparing


                    };
                    if (!validStatusList.Select(x => (int)x).Contains(statusId))
                    {

                        apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
                        apiResponseDto.ErrorData = $"OrderStatusId {statusId} is invalid";
                        return apiResponseDto;
                    }

                }

                else if ((int)OrderStatusEnum.Preparing == previousStatusId)
                {

                    var validStatusList = new List<OrderStatusEnum>() { OrderStatusEnum.Cancelled,OrderStatusEnum.OntheWay


                    };
                    if (!validStatusList.Select(x => (int)x).Contains(statusId))
                    {

                        apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
                        apiResponseDto.ErrorData = $"OrderStatusId {statusId} is invalid";
                        return apiResponseDto;
                    }

                }


                else if ((int)OrderStatusEnum.OntheWay == previousStatusId)
                {

                    var validStatusList = new List<OrderStatusEnum>() { OrderStatusEnum.Cancelled,OrderStatusEnum.Delivered


                    };
                    if (!validStatusList.Select(x => (int)x).Contains(statusId))
                    {

                        apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
                        apiResponseDto.ErrorData = $"OrderStatusId {statusId} is invalid";
                        return apiResponseDto;
                    }

                }


                else if ((int)OrderStatusEnum.Delivered == previousStatusId)
                {

                    var validStatusList = new List<OrderStatusEnum>() { OrderStatusEnum.PendingReturn


                    };
                    if (!validStatusList.Select(x => (int)x).Contains(statusId))
                    {

                        apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
                        apiResponseDto.ErrorData = $"OrderStatusId {statusId} is invalid";
                        return apiResponseDto;
                    }

                }

                else if ((int)OrderStatusEnum.PendingReturn == previousStatusId)
                {

                    var validStatusList = new List<OrderStatusEnum>() {OrderStatusEnum.Returned


                    };
                    if (!validStatusList.Select(x => (int)x).Contains(statusId))
                    {

                        apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
                        apiResponseDto.ErrorData = $"OrderStatusId {statusId} is invalid";
                        return apiResponseDto;
                    }

                }





                else
                {

                    apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
                    apiResponseDto.ErrorData = $"OrderStatusId {statusId} is invalid";
                    return apiResponseDto;

                }




            }
            apiResponseDto.SuccessData = true;
            return apiResponseDto;
        }

        public async Task<ApiResponseDto<string, long>> UpdateOrderStatusAdmin(long orderId, int statusId)
        {
            var apiResponseDto = new ApiResponseDto<string, long>();
            var orderList = await _orderRepository.Filter(x => x.Id == orderId);
            var orderEntityToUpdate = orderList.First();
            orderEntityToUpdate.StatusId = statusId;
            orderEntityToUpdate.LastUpdateDate = DateTime.Now;
            orderEntityToUpdate.LastUpdatedBy = _userId;

            await _orderRepository.Update(orderEntityToUpdate);

            apiResponseDto.SuccessData = orderEntityToUpdate.Id;

            return apiResponseDto;


        }

        public async Task<ApiResponseDto<string, IEnumerable<FilteredOrderDto>>> FilterOrder(FilterOrderRequestDto filterOrderDto)
        {

            var filterOrderModel = _mapper.Map<FilterOrderRequestDto, FilterOrderRequestModel>(filterOrderDto);

            var apiResponseModel = await _orderRepository.FilterOrder(filterOrderModel);
            var apiResponseDto = _mapper.Map<ApiResponseModel<string, IEnumerable<FilteredOrderModel>>, ApiResponseDto<string, IEnumerable<FilteredOrderDto>>>(apiResponseModel);
            return apiResponseDto;
        }

        public async Task<ApiResponseDto<string, IEnumerable<FilteredOrderAdminDto>>> FilterOrderAdmin(FilterOrderRequestAdminDto filterOrderRequestAdminDto)
        {
            var filterOrderRequestAdminModel = _mapper.Map<FilterOrderRequestAdminDto, FilterOrderRequestAdminModel>(filterOrderRequestAdminDto);

            var apiResponseModel = await _orderRepository.FilterOrderAdmin(filterOrderRequestAdminModel);
            var apiResponseDto = _mapper.Map<ApiResponseModel<string, IEnumerable<FilteredOrderAdminModel>>, ApiResponseDto<string, IEnumerable<FilteredOrderAdminDto>>>(apiResponseModel);
            return apiResponseDto;

        }

        public ApiResponseDto<string, IEnumerable<StatusLovAdminDto>> GetStatusLovAdmin()
        {

            var apiResponseDto = new ApiResponseDto<string, IEnumerable<StatusLovAdminDto>>();
            var statusLovAdminDtoList = new List<StatusLovAdminDto>();

            var enumValues = Enum.GetValues(typeof(OrderStatusEnum));
            foreach (OrderStatusEnum enumValue in enumValues)
            {
                StatusLovAdminDto statusLovAdminDto = new StatusLovAdminDto();
                statusLovAdminDto.Id = (int)enumValue;
                statusLovAdminDto.Name = ((OrderStatusEnum)enumValue).GetDisplayNameFromAttr();
                statusLovAdminDtoList.Add(statusLovAdminDto);

            }
            apiResponseDto.SuccessData = statusLovAdminDtoList;
            return apiResponseDto;
        }

        public ApiResponseDto<string, IEnumerable<StatusLovDto>> GetStatusLov()
        {
            var apiResponseDto = new ApiResponseDto<string, IEnumerable<StatusLovDto>>();
            var statusLovDtoList = new List<StatusLovDto>();

            var enumValues = Enum.GetValues(typeof(OrderStatusEnum));
            foreach (OrderStatusEnum enumValue in enumValues)
            {
                StatusLovDto statusLovDto = new StatusLovDto();
                statusLovDto.Id = (int)enumValue;
                statusLovDto.Name = ((OrderStatusEnum)enumValue).GetDisplayNameFromAttr();
                statusLovDtoList.Add(statusLovDto);

            }

            apiResponseDto.SuccessData = statusLovDtoList;
            return apiResponseDto;
        }



























    }
}
