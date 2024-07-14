using FooRestaurantApi.DomainLayer.Model.TaskTakip;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooRestaurantApi.ServicesLayer.Services.Interfaces
{
    public interface ITaskTakipService
    {
        TaskTakipMainModel GetTaskResult(int weekIdStart,int weekIdEnd);
        
    }
}
