using FooRestaurantApi.ServicesLayer.Validator;
using System;
using System.ComponentModel.DataAnnotations;

namespace FooRestaurantApi.ServicesLayer.DTO.FoodCategory
{
    public class CreateFoodCategoryRequestDto
    {
        [MinLengthAttribute(1, ErrorMessage = "The {0} field length must be greater than {1}.")]
        [MaxLength(100, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        public string Name { get; set; }
        [Range(1, 7, ErrorMessage = "The DayId parameter must be a between 1-7.")]
        public int DayId { get; set; }
        [IsInDayOrderValidAttribute]
        public int OrderInDay { get; set; }
    }
}
