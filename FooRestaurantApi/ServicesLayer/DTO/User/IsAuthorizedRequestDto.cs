namespace FooRestaurantApi.ServicesLayer.DTO.User
{
    public class IsAuthorizedRequestDto
    {

        public string Token { get; set; }
        public int ResourceId { get; set; }

    }
}
