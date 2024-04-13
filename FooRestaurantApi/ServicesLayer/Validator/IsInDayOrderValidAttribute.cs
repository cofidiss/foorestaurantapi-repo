using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using FooRestaurantApi.ServicesLayer.DTO.FoodCategory;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FooRestaurantApi.ServicesLayer.Validator
{
    public class IsInDayOrderValidAttribute : ValidationAttribute
    {

        public IsInDayOrderValidAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)

        {
            var inDayOrder = (int)value;

            if (validationContext.ObjectInstance is CreateFoodCategoryRequestDto)
            {

                var dayId = ((CreateFoodCategoryRequestDto)validationContext.ObjectInstance).DayId;

                IFoodCategoryRepository _foodCategoryRepository = (IFoodCategoryRepository)validationContext.GetService(typeof(IFoodCategoryRepository));
                var categoryList = _foodCategoryRepository.Filter(x => x.DayId.Equals(dayId)).Result;


                if (inDayOrder < 1 || categoryList.Count() + 1 < inDayOrder)
                {
                    return new ValidationResult($"InDayOrder {inDayOrder} is not valid.");
                }


            }
            else if (validationContext.ObjectInstance is UpdateFoodCategoryRequestDto)
            {
                var updateFoodCategoryRequestDto = ((UpdateFoodCategoryRequestDto)validationContext.ObjectInstance);
                var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
                var categoryIdStr = httpContextAccessor.HttpContext.Request.RouteValues["id"].ToString();
                long categoryId = -1;
                if (!Int64.TryParse(categoryIdStr, out categoryId))
                {

                    return new ValidationResult($"categoryId {categoryIdStr} is not valid.");


                }
                IFoodCategoryRepository _foodCategoryRepository = (IFoodCategoryRepository)validationContext.GetService(typeof(IFoodCategoryRepository));
                var sourcecategoryelementList = _foodCategoryRepository.Filter(x => x.Id.Equals(categoryId)).Result;
                if (sourcecategoryelementList.Count() == 0)
                {

                    return new ValidationResult($"categoryId {categoryIdStr} is does not exits.");

                }
                var sourcecategoryelement = sourcecategoryelementList.First();


                var destdayCategoryList = _foodCategoryRepository.Filter(x => x.DayId.Equals(updateFoodCategoryRequestDto.DayId)).Result;

                if (sourcecategoryelement.DayId == updateFoodCategoryRequestDto.DayId)
                {
                    if (inDayOrder < 1 || destdayCategoryList.Count() < inDayOrder)
                    {
                        return new ValidationResult($"InDayOrder {inDayOrder} is not valid.");
                    }

                }



                else
                {
                    if (inDayOrder < 1 || destdayCategoryList.Count() + 1 < inDayOrder)
                    {
                        return new ValidationResult($"InDayOrder {inDayOrder} is not valid.");
                    }

                }

            }

            else
            {
                throw new Exception("ObjectInstance should be in type UpdatefoodCategoryRequestDto or CreatefoodCategoryRequestDto. ");
            }






            return ValidationResult.Success;
        }
    }
}
