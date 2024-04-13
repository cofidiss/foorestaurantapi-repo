using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FooRestaurantApi.RepositoryLayer.RepositoryPattern
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> Filter(Expression<Func<T, bool>> filter);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);

    }
}
