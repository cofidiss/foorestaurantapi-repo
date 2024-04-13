using FooRestaurantApi.DomainLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces
{

    public interface ICartRepository : IRepository<FoodEntity>
    {
        Task<bool> IsFoodAvalibleToAddCart(long foodId);
        Task<IList<FoodEntity>> GetAvalibleFoodEntityiesForToday(IEnumerable<long> enumerable);
    }
}
