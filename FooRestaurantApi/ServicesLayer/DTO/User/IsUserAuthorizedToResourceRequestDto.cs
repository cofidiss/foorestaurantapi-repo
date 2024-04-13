namespace FooRestaurantApi.ServicesLayer.DTO.User
{
    public class IsUserAuthorizedToResourceRequestDto
    {
        public long UserId { get; set; }
        public long ResourceId { get; set; }
    }
}
