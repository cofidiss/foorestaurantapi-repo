using System;
using System.Collections.Generic;

namespace FooRestaurantApi.DomainLayer.Model.Order
{
    public class AddOrderRequestModel
    {
        public IEnumerable<FoodAndQuantityForOrderModel> FoodAndQuantityList { get; set; }
        public string OrdersNote { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string PaymentMethod { get; set; }
    }
}
