using System.Collections.Generic;

namespace FooRestaurantApi.ServicesLayer.DTO.Food
{
    public class FoodsForTheCategoryDto
    {
        public IList<FoodInfoDto> FoodList { get; set; }
        public string CategoryName { get; set; }
        public int InDayOrder { get; set; }
    }
}
