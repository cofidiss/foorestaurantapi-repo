using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FooRestaurantApi.ServicesLayer.Validator
{
    public class IsUserIdEnabledAttribute : ValidationAttribute
    {

        public IsUserIdEnabledAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)

        {
            var userId = (long)value;


            IUserRepository _userRepository = (IUserRepository)validationContext.GetService(typeof(IUserRepository));
            var userList = _userRepository.Filter(x => x.Id.Equals(userId) && x.IsEnabled == true).Result;


            if (userList.Count() == 0)
            {
                return new ValidationResult($"UserId {userId} does not  exist.");
            }
            return ValidationResult.Success;

        }
    }
}
