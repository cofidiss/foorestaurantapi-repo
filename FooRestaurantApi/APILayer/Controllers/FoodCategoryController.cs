using FooRestaurantApi.APILayer.Authorization;
using FooRestaurantApi.APILayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.Enums;
using FooRestaurantApi.ServicesLayer.DTO.FoodCategory;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using FooRestaurantApi.ServicesLayer.Validator;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FooRestaurantApi.APILayer.Controllers
{
    [ApiController]

    public class FoodCategoryController : Controller
    {
        public IFoodCategoryService _foodCategoryService { get; set; }
        public FoodCategoryController(IFoodCategoryService foodCategoryService)
        {
            _foodCategoryService = foodCategoryService;

        }




        [RoleIdAuthorize((int)RoleIdEnum.Admin)]

        [HttpPost("api/admin/food/category")]
        public async Task<IActionResult> CreateFoodCategory(CreateFoodCategoryRequestDto createFoodCategoryRequestDto)
        {


            var apiresponseDto = await _foodCategoryService.CreateFoodCategory(createFoodCategoryRequestDto);
            return apiresponseDto.Success ? CreatedAtAction(nameof(GetFoodCategory), new { id = apiresponseDto.SuccessData }, apiresponseDto.SuccessData) : apiresponseDto.CreateCustomResponse();

        }
        [RoleIdAuthorize((int)RoleIdEnum.Admin)]

        [HttpGet("api/admin/food/category/{id}")]
        public async Task<IActionResult> GetFoodCategory(long id)
        {


            var apiresponseDto = await _foodCategoryService.GetFoodCategory(id);

            return apiresponseDto.CreateCustomResponse();
        }
        [RoleIdAuthorize((int)RoleIdEnum.Admin)]
        [HttpPut("api/admin/food/category/{id}")]
        public async Task<IActionResult> UpdateFoodCategory(long id, [FromBody] UpdateFoodCategoryRequestDto updateFoodCategoryRequestDto)
        {

            updateFoodCategoryRequestDto.Id = id;
            var apiresponseDto = await _foodCategoryService.UpdateFoodCategory(updateFoodCategoryRequestDto);
            return apiresponseDto.CreateCustomResponse();


        }
        [RoleIdAuthorize((int)RoleIdEnum.Admin)]
        [HttpDelete("api/admin/food/category/{id}")]
        public async Task<IActionResult> DeleteFoodCategory([IsFoodCategoryExist] long id)
        {


            var apiresponseDto = await _foodCategoryService.DeleteFoodCategory(id);

            return apiresponseDto.CreateCustomResponse();
        }

        [RoleIdAuthorize((int)RoleIdEnum.Admin)]
        [HttpGet("api/admin/food/category/list")]
        public async Task<IActionResult> GetFoodCategoryLovAdmin()
        {
            var apiresponseDto = await _foodCategoryService.GetFoodCategoryLovAdmin();


            return apiresponseDto.CreateCustomResponse();
        }

    }
}

