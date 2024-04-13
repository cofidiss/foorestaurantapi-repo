using AutoMapper;
using FooRestaurantApi.DomainLayer.Entity;

using FooRestaurantApi.DomainLayer.Model.Food;
using FooRestaurantApi.DomainLayer.Model.Order;
using FooRestaurantApi.ServicesLayer.DTO.Food;
using FooRestaurantApi.ServicesLayer.DTO.Order;

namespace FooRestaurantApi.ServicesLayer.Profiles
{
    public class FoodProfile : Profile
    {

        public FoodProfile()
        {
            CreateMap<CreateFoodRequestDto, CreateFoodRequestModel>();

            CreateMap<CreateFoodRequestModel, FoodEntity>().ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => true));
            CreateMap<FoodEntity, FoodDto>();
            CreateMap<UpdateFoodRequestDto, UpdateFoodRequestModel>();
            CreateMap<UpdateFoodRequestModel, FoodEntity>().ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => true)).ForMember(dest => dest.Id, opt => opt.MapFrom(src => 0));
            CreateMap<UpdateFoodRequestDto, UpdateFoodRequestModel>();
            CreateMap<FoodsForTheCategoryAdminModel, FoodsForTheCategoryAdminDto>();
            CreateMap<FoodInfoAdminModel, FoodInfoAdminDto>();
            CreateMap<FoodsForTheCategoryModel, FoodsForTheCategoryDto>();
            CreateMap<FoodInfoModel, FoodInfoDto>();
            CreateMap<OrderDetailFoodModel, OrderDetailFoodDto>();

        }
    }
}

