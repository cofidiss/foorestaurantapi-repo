using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooRestaurantApi.DomainLayer.Entity.TaskTakip
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public decimal EstimatedWorkHour { get; set; }
        public decimal WorkedHour { get; set; }
        public int WeekId { get; set; }
        public decimal PercentCompletion { get; set; }
        public int CategoryId { get; set; }
    }
}
