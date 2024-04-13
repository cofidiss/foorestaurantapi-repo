using System.Collections.Generic;

namespace FooRestaurantApi.ServicesLayer.DTO.Food
{
    public class FoodsForTheCategoryAdminDto
    {
        public IList<FoodInfoAdminDto> FoodList { get; set; }
        public string CategoryName { get; set; }
        public int InDayOrder { get; set; }
        public long CategoryId { get; set; }
    }
}
