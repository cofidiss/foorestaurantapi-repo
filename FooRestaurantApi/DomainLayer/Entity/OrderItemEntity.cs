namespace FooRestaurantApi.DomainLayer.Entity
{
    public class OrderItemEntity : BaseEntity
    {
        public long Id { get; set; }

        public long OrderId { get; set; }
        public long FoodId { get; set; }

        public int Quantity { get; set; }

    }
}
