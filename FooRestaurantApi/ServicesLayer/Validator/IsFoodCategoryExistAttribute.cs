using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FooRestaurantApi.ServicesLayer.Validator
{
    public class IsFoodCategoryExistAttribute : ValidationAttribute
    {

        public IsFoodCategoryExistAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)

        {

            IFoodCategoryRepository _FoodCategoryRepository = (IFoodCategoryRepository)validationContext.GetService(typeof(IFoodCategoryRepository));
            var categoryList = _FoodCategoryRepository.Filter(x => x.Id.Equals((long)value)).Result;


            if (categoryList.Count() == 0)
            {
                return new ValidationResult($"CategoryId {(long)value} does not exist.");
            }
            return ValidationResult.Success;

        }
    }
}
