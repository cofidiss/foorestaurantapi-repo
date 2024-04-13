using System.ComponentModel.DataAnnotations;

namespace FooRestaurantApi.DomainLayer.Model.Enums
{
    public enum OrderStatusEnum
    {
        [Display(Name = "Received")]
        Received = 0,

        [Display(Name = "Confirmed")]
        Confirmed = 1,
        [Display(Name = "Preparing")]
        Preparing = 2,

        [Display(Name = "OntheWay")]
        OntheWay = 3,
        [Display(Name = "Delivered")]
        Delivered = 4,

        [Display(Name = "Cancelled")]
        Cancelled = 5,

        [Display(Name = "PendingReturn")]
        PendingReturn = 6,

        [Display(Name = "Returned")]
        Returned = 7


    }
}
