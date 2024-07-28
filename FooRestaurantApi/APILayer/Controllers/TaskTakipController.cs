using FooRestaurantApi.APILayer.Authorization;
using FooRestaurantApi.APILayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.Enums;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using FooRestaurantApi.ServicesLayer.Validator;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading;
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
        [HttpGet("api/GetTaskResult2")]
        public async Task< IActionResult> GetTaskResult2(int weekIdStart, int weekIdEnd)
        {

            var m1 = async delegate ()
            {
                await Task.Delay(2000);
                //Thread.Sleep(2000);
                Debug.WriteLine("m1");
            };
          var t1 =    Task.Run(m1);
            var t2 = Task.Run(m1);
  
          
            var a =     GetTaskResult3();
       var b =      a.Result;
            return Ok("1");

        }
        [NonAction]
        public async Task<int> GetTaskResult3()
        {
            await Task.Delay(3000).ConfigureAwait(true);
            Debug.WriteLine("GetTaskResult3");
            return 1;

        }


    }
}
