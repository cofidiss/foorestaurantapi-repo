using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using FooRestaurantApi.ServicesLayer.DTO.Food;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FooRestaurantApi.ServicesLayer.Validator
{
    public class IsInCategoryOrderValidAttribute : ValidationAttribute
    {

        public IsInCategoryOrderValidAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)

        {
            var incategoryOrder = (int)value;

            if (validationContext.ObjectInstance is CreateFoodRequestDto)
            {

                var categoryId = ((CreateFoodRequestDto)validationContext.ObjectInstance).CategoryId;

                IFoodRepository _foodRepository = (IFoodRepository)validationContext.GetService(typeof(IFoodRepository));
                var foodList = _foodRepository.Filter(x => x.CategoryId.Equals(categoryId) && x.IsEnabled).Result;


                if (incategoryOrder < 1 || foodList.Count() + 1 < incategoryOrder)
                {
                    return new ValidationResult($"InCategoryOrder {incategoryOrder} is not valid.");
                }

            }
            else if (validationContext.ObjectInstance is UpdateFoodRequestDto)
            {

                var updateFoodRequestDto = ((UpdateFoodRequestDto)validationContext.ObjectInstance);

                IFoodRepository _foodRepository = (IFoodRepository)validationContext.GetService(typeof(IFoodRepository));
                var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
                var foodIdStr = httpContextAccessor.HttpContext.Request.RouteValues["id"].ToString();
                long foodId = -1;
                if (!Int64.TryParse(foodIdStr, out foodId))
                {

                    return new ValidationResult($"foodId {foodIdStr} is not valid.");


                }

                var sourceFoodelementList = _foodRepository.Filter(x => x.Id.Equals(foodId) && x.IsEnabled).Result;
                if (sourceFoodelementList.Count() == 0)
                {
                    return new ValidationResult($"foodId {foodIdStr} does not exits.");


                }

                var sourceFoodelement = sourceFoodelementList.First();
                var destCategoryfoodList = _foodRepository.Filter(x => x.CategoryId.Equals(updateFoodRequestDto.CategoryId) && x.IsEnabled).Result;

                if (sourceFoodelement.CategoryId == updateFoodRequestDto.CategoryId)
                {
                    if (incategoryOrder < 1 || destCategoryfoodList.Count() < incategoryOrder)
                    {
                        return new ValidationResult($"InCategoryOrder {incategoryOrder} is not valid.");
                    }

                }



                else
                {
                    if (incategoryOrder < 1 || destCategoryfoodList.Count() + 1 < incategoryOrder)
                    {
                        return new ValidationResult($"InCategoryOrder {incategoryOrder} is not valid.");
                    }


                }

            }
            else
            {

                throw new Exception("ObjectInstance should be in type CreatefoodRequestDto or UpdatefoodRequestDto ");
            }




            return ValidationResult.Success;
        }
    }
}
