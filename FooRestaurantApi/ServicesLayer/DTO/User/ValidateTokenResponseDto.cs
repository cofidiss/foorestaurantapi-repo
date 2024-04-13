using System.Net;
using System.Text.Json.Serialization;

namespace FooRestaurantApi.ServicesLayer.DTO.User
{
    public class ValidateTokenResponseDto
    {
        public long? UserId { get; set; }
        public string Data { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
