namespace FooRestaurantApi.DomainLayer.Model.FoodCategory
{
    public class UpdateFoodCategoryRequestModel
    {
        public long Id { get; set; }

        public int OrderInDay { get; set; }
        public string CategoryName { get; set; }
        public int DayId { get; set; }
    }
}
