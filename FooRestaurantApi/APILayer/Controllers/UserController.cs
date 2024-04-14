using FooRestaurantApi.APILayer.Authorization;
using FooRestaurantApi.APILayer.Extensions;
using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Model.Enums;
using FooRestaurantApi.ServicesLayer.DTO.User;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using FooRestaurantApi.ServicesLayer.Validator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FooRestaurantApi.APILayer.Controllers
{

    [ApiController]

    public class UserController : Controller
    {
        IUserService _userService { get; }
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("api/user/signup")]
        public async Task<IActionResult> SignUp([IsSignUpRequestValid] SignUpRequestDto signUpRequestDto)
        {
            var apiresponseDto = await _userService.SignUp(signUpRequestDto);
            return apiresponseDto.Success ? CreatedAtAction(nameof(GetUserInfoForUser), new { id = apiresponseDto.SuccessData }) : apiresponseDto.CreateCustomResponse();



        }
        [RoleIdAuthorize((int)RoleIdEnum.StandartUser)]
        [HttpGet("api/user")]
        public async Task<IActionResult> GetUserInfoForUser()
        {
            var apiresponseDto = await _userService.GetUserInfoForUser();
            return apiresponseDto.CreateCustomResponse();



        }
        [HttpPost("api/SignOut")]

        public new  async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(
      CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }


        [AllowAnonymous]
        [HttpPost("api/login")]

        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var apiResponseDto = await _userService.GetUserForUserNameAndPassword(loginRequestDto);
            if (apiResponseDto.Success)
            {
                var claims = new List<Claim>
        {
            new Claim(nameof(UserEntity.Id), apiResponseDto.SuccessData.Id.ToString()),
new Claim(nameof(UserEntity.RoleId), apiResponseDto.SuccessData.RoleId.ToString()),
new Claim(nameof(UserEntity.LastUpdateDate), apiResponseDto.SuccessData.LastUpdateDate?.ToString(CustomCookieAuthenticationEvent.ClaimDateFormat)??String.Empty)

        };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true

                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return Ok();
            }

            return Unauthorized("Wrong username or password");
        }
        [RoleIdAuthorize((int)RoleIdEnum.StandartUser)]
        [HttpPut("api/user")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var apiresponseDto = await _userService.UpdateUser(updateUserDto);
            return apiresponseDto.CreateCustomResponse();
        }
        [RoleIdAuthorize((int)RoleIdEnum.StandartUser)]
        [HttpDelete("api/user")]
        public async Task<IActionResult> DeleteUser()
        {
            var apiresponseDto = await _userService.DeleteUser();
            return apiresponseDto.CreateCustomResponse();
        }
        [RoleIdAuthorize((int)RoleIdEnum.Admin)]
        [HttpGet("api/admin/user/list/{term}")]
        public async Task<IActionResult> GetUserLovAdmin(string term)
        {
            var apiresponseDto = await _userService.GetUserLovAdmin(term);
            return apiresponseDto.CreateCustomResponse();

        }



    }
}

