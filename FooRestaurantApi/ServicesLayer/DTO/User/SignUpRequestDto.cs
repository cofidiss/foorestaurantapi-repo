using FooRestaurantApi.DomainLayer.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace FooRestaurantApi.ServicesLayer.DTO.User
{
    public class SignUpRequestDto
    {
        [MinLengthAttribute(1, ErrorMessage = "The {0} field length must be greater than {1}.")]
        [MaxLength(100, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        public string UserName { get; set; }
        [MinLengthAttribute(1, ErrorMessage = "The {0} field length must be greater than {1}.")]
        [MaxLength(100, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        public string Phone { get; set; }
        [MinLengthAttribute(1, ErrorMessage = "The {0} field length must be greater than {1}.")]
        [MaxLength(100, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        public string Password { get; set; }
        [MinLengthAttribute(1, ErrorMessage = "The {0} field length must be greater than {1}.")]
        [MaxLength(100, ErrorMessage = "The {0} field cannot exceed {1} characters.")]
        public string Address { get; set; }
        public RoleIdEnum RoleId { get; set; }
    }
}
