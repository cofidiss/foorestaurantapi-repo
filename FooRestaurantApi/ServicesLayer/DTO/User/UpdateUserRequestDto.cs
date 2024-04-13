namespace FooRestaurantApi.ServicesLayer.DTO.User
{
    public class UpdateUserRequestDto
    {

        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
