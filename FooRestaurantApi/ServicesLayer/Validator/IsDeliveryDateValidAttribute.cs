using System;
using System.ComponentModel.DataAnnotations;

namespace FooRestaurantApi.ServicesLayer.Validator
{
    public class IsDeliveryDateValidAttribute : ValidationAttribute
    {

        public IsDeliveryDateValidAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)

        {
            var now = DateTime.Now;

            var deliveryDate = (DateTime)value;
            if (!(now.AddMinutes(-5) < deliveryDate && now.Date.AddDays(1) > deliveryDate))
            {

                return new ValidationResult($"DeliveryDate should be between now and last minute of this day.");

            }


            return ValidationResult.Success;

        }
    }
}
