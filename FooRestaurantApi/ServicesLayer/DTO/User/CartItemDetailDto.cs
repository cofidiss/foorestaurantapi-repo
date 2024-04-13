namespace FooRestaurantApi.ServicesLayer.DTO.User
{
    public class CartItemDetailDto
    {


        public long FoodId { get; set; }
        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public byte[] Image { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPirce { get; set; }
    }
}
