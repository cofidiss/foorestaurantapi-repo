using FooRestaurantApi.DomainLayer.Model.Enums;
using System;
using System.Collections.Generic;

namespace FooRestaurantApi.DomainLayer.Model.Order
{
    public class FilteredOrderAdminModel
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public bool IsEditable { get
            { return (StatusId == (int)OrderStatusEnum.Received) || (StatusId == (int)OrderStatusEnum.Confirmed) ||
               (StatusId == (int)OrderStatusEnum.Preparing) || (StatusId == (int)OrderStatusEnum.OntheWay) ||
                 (StatusId == (int)OrderStatusEnum.PendingReturn) || (StatusId == (int)OrderStatusEnum.Delivered);
                    
          
            }
                }
        
        public DateTime DeliveryDate { get; set; }
        public String UserName { get; set; }
        public String Address { get; set; }
        public int StatusId { get; set; }
        public decimal TotalPrice { get; set; }


    }
}
