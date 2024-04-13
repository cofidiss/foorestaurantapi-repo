namespace FooRestaurantApi.ServicesLayer.DTO.User
{
    public class CreateTokenRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Data { get; set; }
    }

}
