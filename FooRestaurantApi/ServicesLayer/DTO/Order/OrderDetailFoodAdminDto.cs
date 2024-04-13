namespace FooRestaurantApi.ServicesLayer.DTO.Order
{
    public class OrderDetailFoodAdminDto
    {


        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public byte[] Image { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }



    }
}
