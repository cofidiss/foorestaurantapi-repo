using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Model.Food;
using FooRestaurantApi.DomainLayer.Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces
{

    public interface IFoodRepository : IRepository<FoodEntity>
    {
        public Task<ApiResponseModel<string, long>> CreateFood(CreateFoodRequestModel createFoodRequestModel);
        Task<ApiResponseModel<string, long>> UpdateFood(UpdateFoodRequestModel updateFoodRequestModel);
        Task<ApiResponseModel<string, string>> DeleteFood(long id);
        Task<ApiResponseModel<string, IEnumerable<FoodsForTheCategoryAdminModel>>> GetFoodByWeekDayAdmin(int id);
        Task<ApiResponseModel<string, IEnumerable<FoodsForTheCategoryModel>>> GetFoodByWeekDay(int id);
    }
}
