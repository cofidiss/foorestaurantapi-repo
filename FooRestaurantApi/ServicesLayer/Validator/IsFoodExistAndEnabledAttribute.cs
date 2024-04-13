using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FooRestaurantApi.ServicesLayer.Validator
{
    public class IsFoodExistAndEnabledAttribute : ValidationAttribute
    {

        public IsFoodExistAndEnabledAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)

        {
            IFoodRepository _FoodRepository = (IFoodRepository)validationContext.GetService(typeof(IFoodRepository));
            var FoodList = _FoodRepository.Filter(x => x.Id.Equals((long)value) && x.IsEnabled).Result;


            if (FoodList.Count() == 0)
            {
                return new ValidationResult($"FoodId {(long)value} does not exist.");
            }

            return ValidationResult.Success;
        }
    }
}
