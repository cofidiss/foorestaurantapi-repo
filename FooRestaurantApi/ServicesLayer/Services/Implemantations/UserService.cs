using AutoMapper;
using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.Response;
using FooRestaurantApi.DomainLayer.Model.User;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using FooRestaurantApi.ServicesLayer.DTO.Response;
using FooRestaurantApi.ServicesLayer.DTO.User;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FooRestaurantApi.ServicesLayer.Services.Implemantations
{
    public class UserService : IUserService
    {

        IMapper _mapper { get; }
        public long? _userId { get; set; }
        IUserRepository _userRepository { get; }
        IHttpContextAccessor _httpContextAccessor;


        public UserService(IMapper mapper, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _userRepository = userRepository;

            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.GetUserIdFromHttpContext();
        }
        public async Task<ApiResponseDto<string, long>> SignUp(SignUpRequestDto signUpRequestDto)
        {
            var apiResponseDto = new ApiResponseDto<string, long>();
            var userEntityToInsert = _mapper.Map<SignUpRequestDto, UserEntity>(signUpRequestDto);

            await _userRepository.Insert(userEntityToInsert);

            apiResponseDto.SuccessData = userEntityToInsert.Id;
            return apiResponseDto;

        }



        public async Task<ApiResponseDto<string, long?>> UpdateUser(UpdateUserDto updateUserDto)
        {
            var apiResponseDto = new ApiResponseDto<string, long?>();
            var userEntityToUpdateList = await _userRepository.Filter(x => x.Id.Equals(_userId) && x.IsEnabled);
            if (userEntityToUpdateList.Count() == 0)
            {
                apiResponseDto.StatusCode = HttpStatusCode.NotFound;
                apiResponseDto.ErrorData = $"UserId {_userId} does not exists.";


                return apiResponseDto;
            }
            var userEntitywtihSameNameList = await _userRepository.Filter(x => x.UserName.Equals(updateUserDto.UserName));
            if (userEntitywtihSameNameList.Count() != 0)
            {
                apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
                apiResponseDto.ErrorData = $"Username {_userId} already exists.";


                return apiResponseDto;
            }

            var userEntityToUpdate = userEntityToUpdateList.First();
            userEntityToUpdate.Address = updateUserDto.Address;
            userEntityToUpdate.UserName = updateUserDto.UserName;
            userEntityToUpdate.Password = updateUserDto.Password;
            userEntityToUpdate.Phone = updateUserDto.Phone;
            userEntityToUpdate.Password = userEntityToUpdate.Password.GetHashedStringAsHex();
            userEntityToUpdate.LastUpdateDate = DateTime.Now;
            userEntityToUpdate.LastUpdatedBy = _userId;

            await _userRepository.Update(userEntityToUpdate);

            apiResponseDto.SuccessData = userEntityToUpdate.Id;
            return apiResponseDto;

        }



        public async Task<ApiResponseDto<string, string>> DeleteUser()
        {

            var apiResponseDto = new ApiResponseDto<string, string?>() { SuccessData = String.Empty };

            var userEntityToSoftdeleteList = (await _userRepository.Filter(x => x.Id.Equals(_userId) && x.IsEnabled));

            if (userEntityToSoftdeleteList.Count() == 0)
            {
                apiResponseDto.StatusCode = HttpStatusCode.BadRequest;

                apiResponseDto.ErrorData = $"UserId {_userId} not found.";
                return apiResponseDto;
            }
            else
            {
                var userEntityToSoftdelete = userEntityToSoftdeleteList.First();
                userEntityToSoftdelete.LastUpdateDate = DateTime.Now;
                userEntityToSoftdelete.LastUpdatedBy = _userId;
                userEntityToSoftdelete.IsEnabled = false;
                await _userRepository.Update(userEntityToSoftdelete);


                return apiResponseDto;

            }




        }

        public async Task<ApiResponseDto<string, UserDtoEski>> UserEntityForUserNameAndPassword(string username, string password)
        {
            throw new Exception();
            var apiResponseDto = new ApiResponseDto<string, bool>();


        }

        public async Task<ApiResponseDto<string, UserDto>> GetUserForUserNameAndPassword(LoginRequestDto loginRequestDto)
        {
            var apiResponseDto = new ApiResponseDto<string, UserDto>();
            var correctEntityList = await _userRepository.Filter(x => x.UserName.Equals(loginRequestDto.UserName) && x.Password.Equals(loginRequestDto.Password.GetHashedStringAsHex()) && x.IsEnabled);

            if (correctEntityList.Count() == 0)
            {

                apiResponseDto.StatusCode = HttpStatusCode.Unauthorized;
                apiResponseDto.ErrorData = "Wrong username or password.";
                return apiResponseDto;

            }
            else
            {
                apiResponseDto.SuccessData = _mapper.Map<UserEntity, UserDto>(correctEntityList.First());

                return apiResponseDto;

            }

        }

        public async Task<ApiResponseDto<string, IEnumerable<UserLovAdminDto>>> GetUserLovAdmin(string term)
        {
            var responseModel = await _userRepository.GetUserLovAdmin(term);
            var responseDto = _mapper.Map<ApiResponseModel<string, IEnumerable<UserLovAdminModel>>, ApiResponseDto<string, IEnumerable<UserLovAdminDto>>>(responseModel);

            return responseDto;
        }

        public async Task<ApiResponseDto<string, UserInfoForUserDto>> GetUserInfoForUser()
        {
            var apiResponseDto = new ApiResponseDto<string, UserInfoForUserDto>();

            var userEntity = (await _userRepository.Filter(x => x.Id == _userId && x.IsEnabled)).First();

            apiResponseDto.SuccessData = _mapper.Map<UserEntity, UserInfoForUserDto>(userEntity);
            return apiResponseDto;
        }



    }
}
