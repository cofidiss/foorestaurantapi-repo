namespace FooRestaurantApi.DomainLayer.Model.Food
{
    public class CreateFoodRequestModel
    {

        public string Name { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public byte[] Image { get; set; }

        public int OrderInCategory { get; set; }
    }
}
