using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using FooRestaurantApi.ServicesLayer.DTO.User;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FooRestaurantApi.ServicesLayer.Validator
{
    public class IsSignUpRequestValidAttribute : ValidationAttribute
    {

        public IsSignUpRequestValidAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)

        {
            var signUpRequestDto = ((SignUpRequestDto)validationContext.ObjectInstance);


            IUserRepository _userRepository = (IUserRepository)validationContext.GetService(typeof(IUserRepository));
            var userList = _userRepository.Filter(x => x.UserName.Equals(signUpRequestDto.UserName)).Result;


            if (userList.Count() != 0)
            {
                return new ValidationResult($"UserName {signUpRequestDto.UserName} already exist.");
            }
            return ValidationResult.Success;

        }
    }
}
