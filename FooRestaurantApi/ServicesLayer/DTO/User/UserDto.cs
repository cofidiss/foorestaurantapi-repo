using System;

namespace FooRestaurantApi.ServicesLayer.DTO.User
{
    public class UserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public int RoleId { get; set; }
        public string Phone { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime? LastUpdateDate { get; set; }

    }
}
