namespace FooRestaurantApi.DomainLayer.Entity
{
    public class FoodCategoryEntity : BaseEntity
    {

        public long Id { get; set; }

        public int OrderInDay { get; set; }
        public string Name { get; set; }
        public int DayId { get; set; }


    }
}
