using System.Collections.Generic;

namespace FooRestaurantApi.ServicesLayer.DTO.User
{
    public class CartDto
    {
        public IEnumerable<CartItemDetailDto> CartItemDetailDtoList { get; set; }
        public string Message { get; set; }
    }
}
