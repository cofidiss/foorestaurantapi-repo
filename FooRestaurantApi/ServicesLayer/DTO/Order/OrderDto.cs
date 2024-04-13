using System;

namespace FooRestaurantApi.ServicesLayer.DTO.Order
{
    public class OrderADto
    {
        public long OrderNo { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }


    }
}
