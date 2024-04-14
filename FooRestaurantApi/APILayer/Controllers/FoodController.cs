using FooRestaurantApi.APILayer.Authorization;
using FooRestaurantApi.APILayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.Enums;
using FooRestaurantApi.ServicesLayer.DTO.Food;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using FooRestaurantApi.ServicesLayer.Validator;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FooRestaurantApi.APILayer.Controllers
{
    [ApiController]

    public class FoodController : Controller
    {
        public IFoodService _foodService { get; set; }
        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;

        }
        [RoleIdAuthorize((int)RoleIdEnum.Admin)]
        [HttpPost("api/admin/food")]
        public async Task<IActionResult> CreateFood(CreateFoodRequestDto createFoodRequestDto)
        {


            var apiResponseDto = await _foodService.CreateFood(createFoodRequestDto);
            return apiResponseDto.Success ? CreatedAtAction(nameof(GetFood), new { id = apiResponseDto.SuccessData }, apiResponseDto.SuccessData) : apiResponseDto.CreateCustomResponse();


        }
        [RoleIdAuthorize((int)RoleIdEnum.Admin)]
        [HttpGet("api/admin/food/{id}")]
        public async Task<IActionResult> GetFood(long id)
        {


            var apiResponseDto = await _foodService.GetFoodAdmin(id);

            return apiResponseDto.CreateCustomResponse();
        }

        [RoleIdAuthorize((int)RoleIdEnum.Admin)]
        [HttpPut("api/admin/food/{id}")]
        public async Task<IActionResult> UpdateFood(long id, [FromBody] UpdateFoodRequestDto updateFoodRequestDto)
        {

            updateFoodRequestDto.Id = id;
            var apiResponseDto = await _foodService.UpdateFood(updateFoodRequestDto);
            return apiResponseDto.Success ? CreatedAtAction(nameof(GetFood), new { id = apiResponseDto.SuccessData }, apiResponseDto.SuccessData) : apiResponseDto.CreateCustomResponse();

        }
        [RoleIdAuthorize((int)RoleIdEnum.Admin)]
        [HttpDelete("api/admin/food/{id}")]
        public async Task<IActionResult> DeleteFood([IsFoodExistAndEnabledAttribute] long id)
        {


            var apiResponseDto = await _foodService.DeleteFood(id);

            return apiResponseDto.CreateCustomResponse();
        }
        [RoleIdAuthorize((int)RoleIdEnum.Admin)]
        [HttpGet("api/admin/food/weekday/{id}")]
        public async Task<IActionResult> GetFoodByWeekDayAdmin([Range(0, 6, ErrorMessage = "The Id parameter must be a between 0-6.")] int id)
        {


            var apiResponseDto = await _foodService.GetFoodByWeekDayAdmin(id);

            return apiResponseDto.CreateCustomResponse();
        }
        [RoleIdAuthorize((int)RoleIdEnum.StandartUser)]
        [HttpGet("api/food/weekday/{id}")]
        public async Task<IActionResult> GetFoodByWeekDay([Range(0, 6, ErrorMessage = "The Id parameter must be a between 0-6.")] int id)
        {
            var apiResponseDto = await _foodService.GetFoodByWeekDay(id);

            return apiResponseDto.CreateCustomResponse();
        }
        [RoleIdAuthorize((int)RoleIdEnum.Admin)]
        [HttpGet("api/admin/day/list")]
        public IActionResult GetDayLovAdmin()
        {

            var apiResponseDto = _foodService.GetDayLovAdmin();
            return apiResponseDto.CreateCustomResponse();


        }

        [HttpGet("api/day/list")]
        public IActionResult GetDayLov()
        {
            var apiResponseDto = _foodService.GetDayLov();

            return apiResponseDto.CreateCustomResponse();


        }


    }
}
