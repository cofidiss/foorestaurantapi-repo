using FooRestaurantApi.ServicesLayer.DTO.Response;
using FooRestaurantApi.ServicesLayer.DTO.User;
using System.Threading.Tasks;

namespace FooRestaurantApi.ServicesLayer.Services.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponseDto<string, object>> AddFoodToCart(long foodId);
        Task<ApiResponseDto<string, CartDto>> GetCart();
        Task<ApiResponseDto<string, string>> RemoveFromCart(long foodId);
    }
}
