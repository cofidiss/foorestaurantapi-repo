using FooRestaurantApi.ServicesLayer.Validator;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace FooRestaurantApi.ServicesLayer.DTO.FoodCategory
{
    public class UpdateFoodCategoryRequestDto
    {
        [BindNever]
        public long Id { get; set; }
        [IsInDayOrderValidAttribute]
        public int OrderInDay { get; set; }
        [MinLengthAttribute(1, ErrorMessage = "The {0} field length must be greater than {1}.")]
        [MaxLength(100, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        public string CategoryName { get; set; }
        [Range(1, 7, ErrorMessage = "The DayId parameter must be a between 1-7.")]
        public int DayId { get; set; }
    }
}
