using AutoMapper;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;

using FooRestaurantApi.ServicesLayer.DTO.Response;
using FooRestaurantApi.ServicesLayer.DTO.User;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FooRestaurantApi.ServicesLayer.Services.Implemantations
{
    public class CartService : ICartService
    {
        public IMapper _mapper { get; }
        IHttpContextAccessor _httpContextAccessor { get; }
        string _cartSessionKey { get; set; }
        ICartRepository _cartRepository { get; }

        IConfiguration _configuration { get; }
        public CartService(IMapper mapper, IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository, IConfiguration configuration)
        {
            _mapper = mapper;

            _httpContextAccessor = httpContextAccessor;
            _cartRepository = cartRepository;
            _configuration = configuration;
            _cartSessionKey = _configuration.GetSection("cartSessionKey").Value;
        }

        public async Task<ApiResponseDto<string, object>> AddFoodToCart(long foodId)
        {


            var session = _httpContextAccessor.HttpContext.Session;


            var apiResponseDto = new ApiResponseDto<string, object>();


            if (!session.Keys.Contains(_cartSessionKey))
            {
                session.SetString(_cartSessionKey, JsonSerializer.Serialize(new List<CartItemDto>()));

            }


            var cartItemsString = session.GetString(_cartSessionKey);


            var cartItemDtoListInSession = JsonSerializer.Deserialize<IEnumerable<CartItemDto>>(cartItemsString);
            var dayOfTheWeek = DateTime.Now.DayOfWeek;


            if (cartItemDtoListInSession.Where(x => x.FoodId == foodId).Count() == 0)
            {
                cartItemDtoListInSession = cartItemDtoListInSession.Append(new CartItemDto() { FoodId = foodId, Quantity = 1 });

            }
            else
            {

                cartItemDtoListInSession.Where(x => x.FoodId == foodId).First().Quantity += 1;

            }
            session.SetString(_cartSessionKey, JsonSerializer.Serialize(cartItemDtoListInSession));
            apiResponseDto.SuccessData = "";
            return apiResponseDto;
        }

        public async Task<ApiResponseDto<string, CartDto>> GetCart()
        {
            var apiResponseDto = new ApiResponseDto<string, CartDto>();
            var session = _httpContextAccessor.HttpContext.Session;

            if (!session.Keys.Contains(_cartSessionKey))
            {
                session.SetString(_cartSessionKey, JsonSerializer.Serialize(new List<CartItemDto>()));

            }


            var cartItemsString = session.GetString(_cartSessionKey);
            var cartItemDtoList = JsonSerializer.Deserialize<IList<CartItemDto>>(cartItemsString);

            var avalibleFoodEntityiesForToday = await _cartRepository.GetAvalibleFoodEntityiesForToday(cartItemDtoList.Select(x => x.FoodId));

            var updatedCartItemDtoList = cartItemDtoList.Where(x => avalibleFoodEntityiesForToday.Select(x => x.Id).Contains(x.FoodId));

            session.SetString(_cartSessionKey, JsonSerializer.Serialize(updatedCartItemDtoList));

            var cartDto = new CartDto();
            var cartItemDetailDtoList = new List<CartItemDetailDto>();
            cartDto.CartItemDetailDtoList = cartItemDetailDtoList;
            if (avalibleFoodEntityiesForToday.Count() != cartItemDtoList.Count())
            {
                cartDto.Message = "Some items have been removed from the basket beacue they are unavalible";

            }

            foreach (var foodEntity in avalibleFoodEntityiesForToday)
            {
                var cartItemDetailDto = new CartItemDetailDto()
                {
                    FoodDescription = foodEntity.Description,
                    UnitPirce = foodEntity.Price,
                    FoodId = foodEntity.Id,
                    FoodName = foodEntity.Name,
                    Image = foodEntity.Image,
                    Quantity = cartItemDtoList.Where(x => x.FoodId == foodEntity.Id).First().Quantity
                };

                cartItemDetailDtoList.Add(cartItemDetailDto);
            }




            apiResponseDto.SuccessData = cartDto;
            return apiResponseDto;
        }

        public async Task<ApiResponseDto<string, string>> RemoveFromCart(long foodId)
        {
            var apiResponseDto = new ApiResponseDto<string, string>();
            var session = _httpContextAccessor.HttpContext.Session;



            if (session.Keys.Contains(_cartSessionKey))
            {

                var cartItemsString = session.GetString(_cartSessionKey);
                var cartItemDtoList = JsonSerializer.Deserialize<IList<CartItemDto>>(cartItemsString);
                if (cartItemDtoList.Where(x => x.FoodId == foodId).Count() == 0)
                {

                    apiResponseDto.ErrorData = $"FoodId {foodId} does not exist in cart.";
                    return apiResponseDto;

                }



                var updatedCartItemDtoList = cartItemDtoList.Where(x => x.FoodId != foodId);
                session.SetString(_cartSessionKey, JsonSerializer.Serialize(updatedCartItemDtoList));
                apiResponseDto.ErrorData = $"FoodId {foodId} removed from cart.";
                return apiResponseDto;
            }
            else
            {

                apiResponseDto.ErrorData = $"FoodId {foodId} does not exist in cart.";
                return apiResponseDto;

            }


        }


    }
}
