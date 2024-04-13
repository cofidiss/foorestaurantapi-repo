using AutoMapper;
using FooRestaurantApi.DomainLayer.Model.Response;
using FooRestaurantApi.ServicesLayer.DTO.Response;
namespace FooRestaurantApi.ServicesLayer.Profiles
{
    public class ResponseProfile : Profile
    {
        public ResponseProfile()
        {
            CreateMap(typeof(ApiResponseModel<,>), typeof(ApiResponseDto<,>));

        }

    }
}
