using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Entity.TaskTakip;
using FooRestaurantApi.DomainLayer.Model.TaskTakip;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces
{

    public interface ITaskTakipRepository : IRepository<TaskEntity>
    {
        TaskTakipMainModel GetTaskResult(int weekIdStart, int weekIdEnd);
    }
}
