using FooRestaurantApi.APILayer.Authorization;
using FooRestaurantApi.APILayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.Enums;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using FooRestaurantApi.ServicesLayer.Validator;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FooRestaurantApi.APILayer.Controllers
{
    [ApiController]

    public class CartController : ControllerBase
    {

        public ICartService _cartService { get; set; }
        public CartController(ICartService cartService)
        {
            _cartService = cartService;

        }
        [RoleIdAuthorize((int)RoleIdEnum.StandartUser)]
        [HttpPost("api/cart")]
        public async Task<IActionResult> AddFoodToCart([IsFoodExistAndEnabledForToday][FromBody] long foodId)
        {
            var apiresponseDto = await _cartService.AddFoodToCart(foodId);

            return apiresponseDto.Success ? CreatedAtAction(nameof(GetCart), new { }) : apiresponseDto.CreateCustomResponse();


        }

        [RoleIdAuthorize((int)RoleIdEnum.StandartUser)]
        [HttpGet("api/cart")]
        public async Task<IActionResult> GetCart()
        {


            var apiresponseDto = await _cartService.GetCart();

            return apiresponseDto.CreateCustomResponse();

        }
        [RoleIdAuthorize((int)RoleIdEnum.StandartUser)]
        [HttpDelete("api/cart/{foodId}")]
        public async Task<IActionResult> RemoveFromCart(long foodId)
        {


            var apiresponseDto = await _cartService.RemoveFromCart(foodId);

            return apiresponseDto.CreateCustomResponse();

        }

    }
}
