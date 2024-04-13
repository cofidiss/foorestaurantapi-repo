using FooRestaurantApi.DomainLayer.Entity;

namespace FooRestaurantApi.DomainLayer.Model.Food
{
    public class FoodAndCategoryModel
    {


        public FoodAndCategoryModel(FoodEntity Food, FoodCategoryEntity FoodCategory)
        {
            CategoryId = FoodCategory.Id;
            FoodId = Food?.Id;
            OrderInDay = FoodCategory.OrderInDay;
            CategoryName = FoodCategory.Name;
            DayId = FoodCategory.DayId;
            Price = Food?.Price;
            FoodName = Food?.Name;
            FoodDescription = Food?.Description;
            Image = Food?.Image;
            OrderInCategory = Food?.OrderInCategory;

        }

        public long CategoryId { get; set; }

        public long? FoodId { get; set; }

        public int OrderInDay { get; set; }
        public string CategoryName { get; set; }
        public int DayId { get; set; }

        public decimal? Price { get; set; }

        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public byte[] Image { get; set; }
        public int? OrderInCategory { get; set; }
    }
}
