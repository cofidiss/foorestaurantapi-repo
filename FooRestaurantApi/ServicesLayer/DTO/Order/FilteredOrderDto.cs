using System;

namespace FooRestaurantApi.ServicesLayer.DTO.Order
{
    public class FilteredOrderDto
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }


        public DateTime DeliveryDate { get; set; }
        public bool IsEditable { get; set; }
        public String Address { get; set; }
        public String StatusName { get; set; }
        public int StatusId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
