using FooRestaurantApi.RepositoryLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FooRestaurantApi.RepositoryLayer.RepositoryPattern
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region property  
        private readonly PostgreDbContext _postgreDbContext;
        private DbSet<T> entities;
        #endregion

        #region Constructor  
        public Repository(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
            entities = _postgreDbContext.Set<T>();
        }
        #endregion




        public async Task<IEnumerable<T>> Filter(Expression<Func<T, bool>> filter)
        {

            return await entities.Where(filter).ToArrayAsync();
        }

        public async Task Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await entities.AddAsync(entity);
            await _postgreDbContext.SaveChangesAsync();
        }



        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            entities.Update(entity);
            await _postgreDbContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            await _postgreDbContext.SaveChangesAsync();
        }
    }
}
