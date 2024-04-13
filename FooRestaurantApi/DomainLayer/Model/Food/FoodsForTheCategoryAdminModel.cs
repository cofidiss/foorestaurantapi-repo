using System.Collections.Generic;

namespace FooRestaurantApi.DomainLayer.Model.Food
{
    public class FoodsForTheCategoryAdminModel
    {
        public IList<FoodInfoAdminModel> FoodList { get; set; }
        public string CategoryName { get; set; }
        public int InDayOrder { get; set; }
        public long CategoryId { get; set; }
    }
}
