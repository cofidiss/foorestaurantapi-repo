using AutoMapper;
using FooRestaurantApi.DomainLayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.Enums;
using FooRestaurantApi.DomainLayer.Model.Order;
using FooRestaurantApi.ServicesLayer.DTO.Order;
using FooRestaurantApi.ServicesLayer.DTO.User;


namespace FooRestaurantApi.ServicesLayer.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {


            CreateMap<FilterOrderRequestAdminDto, FilterOrderRequestAdminModel>();
            CreateMap<FilterOrderRequestDto, FilterOrderRequestModel>();
            CreateMap<OrderDetailModel, OrderDetailDto>().ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => ((OrderStatusEnum)src.StatusId).GetDisplayNameFromAttr()));

            CreateMap<AddOrderRequestDto, AddOrderRequestModel>();
            CreateMap<CartItemDto, FoodAndQuantityForOrderModel>();
            CreateMap<FilteredOrderModel, FilteredOrderDto>().ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => ((OrderStatusEnum)src.StatusId).GetDisplayNameFromAttr()));
            CreateMap<FilteredOrderAdminModel, FilteredOrderAdminDto>().ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => ((OrderStatusEnum)src.StatusId).GetDisplayNameFromAttr()));

            CreateMap<OrderDetailAdminModel, OrderDetailAdminDto>().ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => ((OrderStatusEnum)src.StatusId).GetDisplayNameFromAttr()));
            CreateMap<OrderDetailFoodModel, OrderDetailFoodDto>();
            CreateMap<OrderDetailFoodAdminModel, OrderDetailFoodAdminDto>();
        }
    }
}
