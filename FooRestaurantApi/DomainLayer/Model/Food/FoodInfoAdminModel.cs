namespace FooRestaurantApi.DomainLayer.Model.Food
{
    public class FoodInfoAdminModel
    {
        public long Id { get; set; }
        public decimal Price { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public byte[] Image { get; set; }
        public int OrderInCategory { get; set; }

    }
}
