using System;

namespace FooRestaurantApi.DomainLayer.Entity
{
    public class BaseEntity
    {
        public DateTime CreationDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public long? LastUpdatedBy { get; set; }


    }
}