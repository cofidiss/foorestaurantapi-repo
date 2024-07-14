using FooRestaurantApi.APILayer.Authorization;
using FooRestaurantApi.APILayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.Enums;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using FooRestaurantApi.ServicesLayer.Validator;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FooRestaurantApi.APILayer.Controllers
{
    [ApiController]

    public class TaskTakipController : Controller
    {

        public ITaskTakipService _taskTakipService { get; set; }
        public TaskTakipController(ITaskTakipService taskTakipService)
        {
            _taskTakipService = taskTakipService;

        }
      
        [HttpGet("api/GetTaskResult")]
        public IActionResult GetTaskResult( int  weekIdStart, int weekIdEnd)
        {
            var apiresponseDto =  _taskTakipService.GetTaskResult(weekIdStart, weekIdEnd);
            return View("TaskResult.cshtml", apiresponseDto);
   


        }

    }
}
