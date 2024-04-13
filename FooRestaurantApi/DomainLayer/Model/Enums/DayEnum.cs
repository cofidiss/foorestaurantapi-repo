using System.ComponentModel.DataAnnotations;

namespace FooRestaurantApi.DomainLayer.Model.Enums
{
    public enum DayEnum
    {
        [Display(Name = "Monday")]
        Monday = 0,
        [Display(Name = "TuesDay")]
        TuesDay = 1,
        [Display(Name = "Wednesday")]
        Wednesday = 3,
        [Display(Name = "Thursday")]
        Thursday = 4,
        [Display(Name = "Friday")]
        Friday = 5,
        [Display(Name = "Saturday")]
        Saturday = 6,
        [Display(Name = "Sunday")]
        Sunday = 7

    }
}
