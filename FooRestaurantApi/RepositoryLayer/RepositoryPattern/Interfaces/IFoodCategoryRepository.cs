using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Model.FoodCategory;
using FooRestaurantApi.DomainLayer.Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces
{

    public interface IFoodCategoryRepository : IRepository<FoodCategoryEntity>
    {
        Task<ApiResponseModel<string, long>> CreateFoodCategory(CreateFoodCategoryRequestModel createFoodCategoryRequestModel);
        Task<ApiResponseModel<string, long>> UpdateFoodCategory(UpdateFoodCategoryRequestModel updateFoodCategoryRequestModel);



        Task<ApiResponseModel<string, string>> DeleteFoodCategory(long id);
        Task<ApiResponseModel<string, IEnumerable<FoodCategoryLovAdminModel>>> GetFoodCategoryLovAdmin();
    }
}
