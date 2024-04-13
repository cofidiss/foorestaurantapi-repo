using System;
using System.Collections.Generic;

namespace FooRestaurantApi.DomainLayer.Model.Order
{
    public class OrderDetailModel
    {
        public long OrderNo { get; set; }

        public string Address { get; set; }

        public DateTime DeliveryDate { get; set; }
        public DateTime OrderDate { get; set; }
        public int StatusId { get; set; }

        public decimal TotalPrice { get; set; }

        public IList<OrderDetailFoodModel> OrderDetailFoodList { get; set; }
    }
}
