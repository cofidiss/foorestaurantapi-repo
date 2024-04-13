using AutoMapper;
using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Model.FoodCategory;
using FooRestaurantApi.ServicesLayer.DTO.FoodCategory;

namespace FooRestaurantApi.ServicesLayer.Profiles
{
    public class FoodCategoryProfile : Profile
    {
        public FoodCategoryProfile()
        {
            CreateMap<CreateFoodCategoryRequestDto, CreateFoodCategoryRequestModel>();
            CreateMap<CreateFoodCategoryRequestModel, FoodCategoryEntity>();
            CreateMap<UpdateFoodCategoryRequestDto, UpdateFoodCategoryRequestModel>();
            CreateMap<UpdateFoodCategoryRequestModel, FoodCategoryEntity>();
            CreateMap<FoodCategoryLovAdminModel, FoodCategoryLovAdminDto>();
            CreateMap<FoodCategoryEntity, FoodCategoryDto>();
        }


    }
}
