using System;

namespace FooRestaurantApi.DomainLayer.Entity
{
    public class OrderEntity : BaseEntity
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        public string Note { get; set; }
        public int StatusId { get; set; }
        public string PaymentMethod { get; set; }
        public string Address { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
