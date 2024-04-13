using System;
using System.Collections.Generic;

namespace FooRestaurantApi.DomainLayer.Model.Order
{
    public class OrderDetailAdminModel
    {
        public long OrderNo { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime OrderDate { get; set; }
        public int StatusId { get; set; }

        public decimal TotalPrice { get; set; }

        public IList<OrderDetailFoodAdminModel> OrderDetailFoodList { get; set; }
    }
}
