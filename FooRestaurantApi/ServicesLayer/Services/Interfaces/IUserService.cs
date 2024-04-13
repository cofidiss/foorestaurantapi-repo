using FooRestaurantApi.ServicesLayer.DTO.Response;
using FooRestaurantApi.ServicesLayer.DTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooRestaurantApi.ServicesLayer.Services.Interfaces
{
    public interface IUserService
    {

        Task<ApiResponseDto<string, long>> SignUp(SignUpRequestDto signUpRequestDto);
        Task<ApiResponseDto<string, UserDto>> GetUserForUserNameAndPassword(LoginRequestDto loginRequestDto);


        Task<ApiResponseDto<string, long?>> UpdateUser(UpdateUserDto updateUserDto);
        Task<ApiResponseDto<string, UserInfoForUserDto>> GetUserInfoForUser();


        Task<ApiResponseDto<string, string>> DeleteUser();
        Task<ApiResponseDto<string, IEnumerable<UserLovAdminDto>>> GetUserLovAdmin(string term);
    }
}
