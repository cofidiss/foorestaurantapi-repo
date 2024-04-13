namespace FooRestaurantApi.DomainLayer.Model.User
{
    public class UpdateUserRequestModel
    {
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
    }
}
