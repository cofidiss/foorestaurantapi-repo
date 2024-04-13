using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace FooRestaurantApi.APILayer.Filters
{
    public class IsOrderStatusUpdateValidForAdminFilter : IAsyncActionFilter
    {
        IOrderService _orderService;

        public IsOrderStatusUpdateValidForAdminFilter(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var orderId = (long)context.ActionArguments["orderId"];
            var statusId = (int)context.ActionArguments["statusId"];

            var apiresponseDto = await _orderService.IsOrderStatusUpdateValidForAdmin(orderId, statusId);
            if (apiresponseDto.Success)
            {
                await next();
                return;
            }
            else
            {
                context.Result = new BadRequestObjectResult(apiresponseDto.ErrorData);
                return;

            }

        }





    }
}
