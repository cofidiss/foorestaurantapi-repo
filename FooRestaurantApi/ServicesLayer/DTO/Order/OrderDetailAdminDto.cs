using System;
using System.Collections.Generic;

namespace FooRestaurantApi.ServicesLayer.DTO.Order
{
    public class OrderDetailAdminDto
    {

        public long OrderNo { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string StatusName { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public IList<OrderDetailFoodAdminDto> OrderDetailFoodList { get; set; }

    }
}
