namespace FooRestaurantApi.DomainLayer.Model.Food
{
    public class UpdateFoodRequestModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public long CategoryId { get; set; }

        public decimal Price { get; set; }

        public byte[] Image { get; set; }

        public int OrderInCategory { get; set; }
    }
}
