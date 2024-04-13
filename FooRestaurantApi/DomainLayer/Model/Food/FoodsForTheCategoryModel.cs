using System.Collections.Generic;

namespace FooRestaurantApi.DomainLayer.Model.Food
{
    public class FoodsForTheCategoryModel
    {
        public IList<FoodInfoModel> FoodList { get; set; }
        public string CategoryName { get; set; }
        public int InDayOrder { get; set; }
    }
}
