using FooRestaurantApi.ServicesLayer.Validator;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace FooRestaurantApi.ServicesLayer.DTO.Food
{
    public class UpdateFoodRequestDto
    {
        [BindNever]
        public long Id { get; set; }
        [MaxLength(100, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        [MinLengthAttribute(1, ErrorMessage = "The {0} field length must be greater than {1}.")]
        public string Name { get; set; }
        [MaxLength(100, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        public string? Description { get; set; }
        [IsFoodCategoryExist]
        public long CategoryId { get; set; }
        [PositiveNumber]
        public decimal Price { get; set; }
        [MinLengthAttribute(1, ErrorMessage = "The {0} field length must be greater than {1}.")]
        [MaxLengthAttribute(int.MaxValue, ErrorMessage = "The {0} field length must be less than {1}.")]
        public byte[] Image { get; set; }
        [IsInCategoryOrderValid]

        public int OrderInCategory { get; set; }
    }
}
