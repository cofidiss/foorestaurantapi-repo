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
        Wednesday = 2,
        [Display(Name = "Thursday")]
        Thursday = 3,
        [Display(Name = "Friday")]
        Friday = 4,
        [Display(Name = "Saturday")]
        Saturday = 5,
        [Display(Name = "Sunday")]
        Sunday = 6

    }
}
