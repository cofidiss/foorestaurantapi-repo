using FooRestaurantApi.ServicesLayer.Validator;
using System;
using System.ComponentModel.DataAnnotations;

namespace FooRestaurantApi.ServicesLayer.DTO.Order
{
    public class AddOrderRequestDto
    {
        [MaxLength(100, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        public string OrdersNote { get; set; }
        [IsDeliveryDateValidAttribute]
        public DateTime DeliveryDate { get; set; }
        [MaxLength(100, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        [MinLength(1, ErrorMessage = "The {0} field length must be greater than {1}.")]
        public string PaymentMethod { get; set; }

    }
}
