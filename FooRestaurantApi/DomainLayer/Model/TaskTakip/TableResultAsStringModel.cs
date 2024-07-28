using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooRestaurantApi.DomainLayer.Model.TaskTakip
{
    public class TableResultAsStringModel
    {

        public int WeekId { get; set; }

        public string Category1Result { get; set; }
        public string Category2Result { get; set; }
        public string Category3Result { get; set; }
        public string Category4Result { get; set; }
    
        public string Category5Result { get; set; }
        public string Category6Result { get; set; }
        public string Category7Result { get; set; }
        public string Category8Result { get; set; }
        public string TotalEstimatedWorkHour { get; set; }
        public string TotalWorkedHour { get; set; }
        public string TotalPercentCompletion { get; set; }
    }
}
