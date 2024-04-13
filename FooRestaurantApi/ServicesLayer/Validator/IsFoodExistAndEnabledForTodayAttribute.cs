using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FooRestaurantApi.ServicesLayer.Validator
{
    public class IsFoodExistAndEnabledForTodayAttribute : ValidationAttribute
    {

        public IsFoodExistAndEnabledForTodayAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)

        {
            IFoodRepository _FoodRepository = (IFoodRepository)validationContext.GetService(typeof(IFoodRepository));
            IFoodCategoryRepository _categoryRepository = (IFoodCategoryRepository)validationContext.GetService(typeof(IFoodCategoryRepository));

            var FoodList = _FoodRepository.Filter(x => x.Id.Equals((long)value) && x.IsEnabled).Result;


            if (FoodList.Count() == 0)
            {
                return new ValidationResult($"FoodId {(long)value} is not avalible.");
            }
            else
            {
                if (_categoryRepository.Filter(x => x.Id == FoodList.First().CategoryId && x.DayId == ((int)DateTime.Now.DayOfWeek)).Result.Count() == 0)
                {
                    return new ValidationResult($"FoodId {(long)value} is not avalible.");

                }

            }

            return ValidationResult.Success;
        }
    }
}
