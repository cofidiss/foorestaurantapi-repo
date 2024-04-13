using FooRestaurantApi.ServicesLayer.DTO.FoodCategory;
using FooRestaurantApi.ServicesLayer.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooRestaurantApi.ServicesLayer.Services.Interfaces
{
    public interface IFoodCategoryService
    {
        Task<ApiResponseDto<string, long>> CreateFoodCategory(CreateFoodCategoryRequestDto createFoodCategoryRequestDto);
        Task<ApiResponseDto<string, FoodCategoryDto>> GetFoodCategory(long id);
        Task<ApiResponseDto<string, long>> UpdateFoodCategory(UpdateFoodCategoryRequestDto updateFoodCategoryRequestDto);
        Task<ApiResponseDto<string, string>> DeleteFoodCategory(long id);
        Task<ApiResponseDto<string, IEnumerable<FoodCategoryLovAdminDto>>> GetFoodCategoryLovAdmin();
    }
}
