namespace FooRestaurantApi.ServicesLayer.DTO.Food
{
    public class FoodDto
    {
        public long Id { get; set; }
        public decimal Price { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public bool IsEnabled { get; set; }
        public byte[] Image { get; set; }
        public int OrderInCategory { get; set; }
    }
}
