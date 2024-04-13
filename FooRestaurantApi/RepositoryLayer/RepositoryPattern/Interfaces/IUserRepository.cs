using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Model.Response;
using FooRestaurantApi.DomainLayer.Model.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<ApiResponseModel<string, IEnumerable<UserLovAdminModel>>> GetUserLovAdmin(string term);
    }
}
