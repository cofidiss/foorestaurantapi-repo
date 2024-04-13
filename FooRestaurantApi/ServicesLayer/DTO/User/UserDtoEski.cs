using System;

namespace FooRestaurantApi.ServicesLayer.DTO.User
{
    public class UserDtoEski
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long TokenExpireHour { get; set; }
        public DateTime CreationDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public long? LastUpdatedBy { get; set; }

    }
}
