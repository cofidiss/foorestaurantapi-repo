
using FooRestaurantApi.ServicesLayer.DTO.Food;
using FooRestaurantApi.ServicesLayer.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooRestaurantApi.ServicesLayer.Services.Interfaces
{
    public interface IFoodService
    {
        public Task<ApiResponseDto<string, long>> CreateFood(CreateFoodRequestDto createFoodRequestDto);
        Task<ApiResponseDto<string, IEnumerable<FoodDto>>> GetFoodAdmin(long id);
        Task<ApiResponseDto<string, long>> UpdateFood(UpdateFoodRequestDto updateFoodRequestDto);
        Task<ApiResponseDto<string, string>> DeleteFood(long id);
        Task<ApiResponseDto<string, IEnumerable<FoodsForTheCategoryAdminDto>>> GetFoodByWeekDayAdmin(int id);
        Task<ApiResponseDto<string, IEnumerable<FoodsForTheCategoryDto>>> GetFoodByWeekDay(int id);
        ApiResponseDto<string, IEnumerable<DayLovAdminDto>> GetDayLovAdmin();
        ApiResponseDto<string, IEnumerable<DayLovDto>> GetDayLov();
    }
}
