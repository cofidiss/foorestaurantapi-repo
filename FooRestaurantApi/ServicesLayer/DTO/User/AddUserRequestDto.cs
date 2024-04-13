namespace FooRestaurantApi.ServicesLayer.DTO.User
{
    public class AddUserRequestDto
    {

        public string UserName { get; set; }

        public string Password { get; set; }
        public long TokenExpireHour { get; set; }
    }
}
