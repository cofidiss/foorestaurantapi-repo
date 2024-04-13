using System;
using System.Collections.Generic;

namespace FooRestaurantApi.ServicesLayer.DTO.Order
{
    public class OrderDetailDto
    {

        public long OrderNo { get; set; }

        public string Address { get; set; }

        public DateTime DeliveryDate { get; set; }
        public DateTime OrderDate { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public decimal TotalPrice { get; set; }

        public IList<OrderDetailFoodDto> OrderDetailFoodList { get; set; }

    }
}
