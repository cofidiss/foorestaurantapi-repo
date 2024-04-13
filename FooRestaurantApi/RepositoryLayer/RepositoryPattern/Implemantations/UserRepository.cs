using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Model.Response;
using FooRestaurantApi.DomainLayer.Model.User;
using FooRestaurantApi.RepositoryLayer.Context;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FooRestaurantApi.RepositoryLayer.RepositoryPattern.Implemantations
{
    public class UserRepository : Repository<UserEntity>, IUserRepository
    {
        PostgreDbContext _dbContext { get; }

        public UserRepository(PostgreDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<ApiResponseModel<string, IEnumerable<UserLovAdminModel>>> GetUserLovAdmin(string term)
        {

            var apiResponseModel = new ApiResponseModel<string, IEnumerable<UserLovAdminModel>>();
            var userLovList = await (from user in _dbContext.Set<UserEntity>()
                                     where EF.Functions.Like(user.UserName, $"%{term}%") 
                     
                                     select new UserLovAdminModel() { Id = user.Id, Name = user.UserName }).ToListAsync();



            apiResponseModel.SuccessData = userLovList;
            return apiResponseModel;


        }
    }
}
