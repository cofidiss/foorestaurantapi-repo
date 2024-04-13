using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.RepositoryLayer.Context;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FooRestaurantApi.RepositoryLayer.RepositoryPattern.Implemantations
{
    public class CartRepository : Repository<FoodEntity>, ICartRepository
    {
        PostgreDbContext _dbContext { get; }

        public CartRepository(PostgreDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<bool> IsFoodAvalibleToAddCart(long foodId)
        {
            var foodList = await (from food in _dbContext.Set<FoodEntity>()
                                  join category in _dbContext.Set<FoodCategoryEntity>() on food.CategoryId equals category.Id
                                  where food.Id == foodId && category.Id == (int)(DateTime.Now.DayOfWeek)
                                  select food).ToListAsync();
            return foodList.Count() != 0;
        }

        public async Task<IList<FoodEntity>> GetAvalibleFoodEntityiesForToday(IEnumerable<long> foodIds)
        {
            var foodList = await (from food in _dbContext.Set<FoodEntity>()
                                  join category in _dbContext.Set<FoodCategoryEntity>() on food.CategoryId equals category.Id
                                  where foodIds.Contains(food.Id) && category.DayId == (int)(DateTime.Now.DayOfWeek)
                                  select food).ToListAsync();
            return foodList;

        }
    }
}
