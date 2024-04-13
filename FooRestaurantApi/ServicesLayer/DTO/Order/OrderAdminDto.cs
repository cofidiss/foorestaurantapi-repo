using System;

namespace FooRestaurantApi.ServicesLayer.DTO.Order
{
    public class OrderAdminDto
    {
        public long OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderOwnerUserName { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
