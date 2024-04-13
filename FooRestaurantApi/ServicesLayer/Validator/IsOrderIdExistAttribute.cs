using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FooRestaurantApi.ServicesLayer.Validator
{
    public class IsOrderIdExistAttribute : ValidationAttribute
    {

        public IsOrderIdExistAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)

        {

            IOrderRepository _orderRepository = (IOrderRepository)validationContext.GetService(typeof(IOrderRepository));
            var orderList = _orderRepository.Filter(x => x.Id.Equals((long)value)).Result;


            if (orderList.Count() == 0)
            {
                return new ValidationResult($"orderId {(long)value} does not exist.");
            }
            return ValidationResult.Success;

        }
    }
}
