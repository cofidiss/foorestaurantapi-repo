using AutoMapper;
using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.Enums;
using FooRestaurantApi.DomainLayer.Model.User;
using FooRestaurantApi.ServicesLayer.DTO.User;
using System;

namespace FooRestaurantApi.ServicesLayer.Profiles
{
    public class UserProfile : Profile
    {

        public UserProfile()
        {
            CreateMap<UserEntity, UserDto>();
            CreateMap<SignUpRequestDto, SignUpRequestModel>();
            CreateMap<SignUpRequestDto, UserEntity>().ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.GetHashedStringAsHex())).ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (int)RoleIdEnum.StandartUser))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
                 .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => -1));
            CreateMap<UserLovAdminModel, UserLovAdminDto>();
            CreateMap<UserEntity, UserInfoForUserDto>();

        }
    }
}
