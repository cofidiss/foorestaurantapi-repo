namespace FooRestaurantApi.DomainLayer.Model.User
{
    public class AddUserRequestModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        public long TokenExpireHour { get; set; }
    }
}
