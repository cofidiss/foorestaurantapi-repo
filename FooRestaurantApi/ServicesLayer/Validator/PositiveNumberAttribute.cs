using System.ComponentModel.DataAnnotations;

namespace FooRestaurantApi.ServicesLayer.Validator
{
    public class PositiveNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)


        {
            decimal decimalValue = (decimal)value;

            if (decimalValue <= 0)
            {
                return new ValidationResult("Value must be a positive number.");
            }

            return ValidationResult.Success;
        }
    }
}
