using System;

namespace FooRestaurantApi.DomainLayer.Model.Order
{
    public class FilterOrderRequestAdminModel
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public long? OrderId { get; set; }
        public DateTime? OrderDateStart { get; set; }
        public DateTime? OrderDateEnd { get; set; }
        public int? StatusId { get; set; }
        public long? UserId { get; set; }
        public DateTime? DeliveryDateStart { get; set; }
        public DateTime? DeliveryDateEnd { get; set; }
        public decimal? PriceStart { get; set; }
        public decimal? PriceEnd { get; set; }
        public String Address { get; set; }
    }
}
