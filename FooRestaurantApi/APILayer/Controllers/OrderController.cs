using FooRestaurantApi.APILayer.Authorization;
using FooRestaurantApi.APILayer.Extensions;
using FooRestaurantApi.APILayer.Filters;
using FooRestaurantApi.DomainLayer.Model.Enums;
using FooRestaurantApi.ServicesLayer.DTO.Order;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using FooRestaurantApi.ServicesLayer.Validator;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FooRestaurantApi.APILayer.Controllers
{
    [ApiController]

    public class OrderController : ControllerBase
    {

        public IOrderService _orderService { get; set; }
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;

        }
        [RoleIdAuthorize((int)RoleIdEnum.StandartUser)]
        [HttpPost("api/order")]
        public async Task<IActionResult> AddOrder(AddOrderRequestDto addOrderRequestDto)
        {
            var apiresponseDto = await _orderService.AddOrder(addOrderRequestDto);
            return apiresponseDto.Success ? CreatedAtAction(nameof(GetOrderDetail), new { id = apiresponseDto.SuccessData }, apiresponseDto.SuccessData) : apiresponseDto.CreateCustomResponse();

        }

        [RoleIdAuthorize((int)RoleIdEnum.StandartUser)]
        [HttpGet("api/order/detail/{id}")]
        public async Task<IActionResult> GetOrderDetail([IsOrderIdExist] long id)
        {


            var apiresponseDto = await _orderService.GetOrderDetail(id);

            return apiresponseDto.CreateCustomResponse();

        }
        [RoleIdAuthorize((int)RoleIdEnum.Admin)]
        [HttpGet("api/admin/order/detail/{id}")]
        public async Task<IActionResult> GetOrderDetailAdmin([IsOrderIdExist] long id)
        {


            var apiresponseDto = await _orderService.GetOrderDetailAdmin(id);

            return apiresponseDto.CreateCustomResponse();

        }
        [RoleIdAuthorize((int)RoleIdEnum.StandartUser)]
        [ServiceFilter(typeof(IsOrderStatusUpdateValidForUserFilter))]

        [HttpPut("api/order/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(long orderId, [FromBody] int statusId)
        {
            var apiresponseDto = await _orderService.UpdateOrderStatus(orderId, statusId);


            return apiresponseDto.CreateCustomResponse();


        }
        [RoleIdAuthorize((int)RoleIdEnum.Admin)]
        [ServiceFilter(typeof(IsOrderStatusUpdateValidForAdminFilter))]
        [HttpPut("api/admin/order/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatusAdmin(long orderId, [FromBody] int statusId)
        {


            var apiresponseDto = await _orderService.UpdateOrderStatusAdmin(orderId, statusId);

            return apiresponseDto.CreateCustomResponse();


        }
        [RoleIdAuthorize((int)RoleIdEnum.StandartUser)]
        [HttpGet("api/order")]
        public async Task<IActionResult> FilterOrder([FromQuery] FilterOrderRequestDto filterOrderDto)
        {


            var apiresponseDto = await _orderService.FilterOrder(filterOrderDto);

            return apiresponseDto.CreateCustomResponse();


        }
        [RoleIdAuthorize((int)RoleIdEnum.Admin)]

        [HttpGet("api/admin/order")]
        public async Task<IActionResult> FilterOrderAdmin([FromQuery] FilterOrderRequestAdminDto filterOrderRequestAdminDto)
        {


            var apiresponseDto = await _orderService.FilterOrderAdmin(filterOrderRequestAdminDto);

            return apiresponseDto.CreateCustomResponse();


        }


        [RoleIdAuthorize((int)RoleIdEnum.Admin)]
        [HttpGet("api/admin/order/status/list")]
        public IActionResult GetStatusLovAdmin()
        {


            var apiresponseDto = _orderService.GetStatusLovAdmin();

            return apiresponseDto.CreateCustomResponse();


        }
        [RoleIdAuthorize((int)RoleIdEnum.StandartUser)]
        [HttpGet("api/order/status/list")]
        public IActionResult GetStatusLov()
        {


            var apiresponseDto = _orderService.GetStatusLov();


            return apiresponseDto.CreateCustomResponse();

        }




    }
}
