using FooRestaurantApi.DomainLayer.Model.Enums;
using System;

namespace FooRestaurantApi.DomainLayer.Model.Order
{
    public class FilteredOrderModel
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public bool IsEditable { get {

                return (StatusId == (int)OrderStatusEnum.Received || StatusId == (int)OrderStatusEnum.Confirmed);
        
            }}
        public DateTime DeliveryDate { get; set; }

        public String Address { get; set; }
        public int StatusId { get; set; }
        public decimal TotalPrice { get; set; }


    }
}
