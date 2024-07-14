using FooRestaurantApi.DomainLayer.Entity.TaskTakip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooRestaurantApi.DomainLayer.Model.TaskTakip
{
    public class TaskTakipMainModel
    {
        public List<TableResultAsStringModel> TableResultAsStringModelList { get; set; }
        public List<CategoryEntity> CategoryEntityList { get; set; }
    }
}
