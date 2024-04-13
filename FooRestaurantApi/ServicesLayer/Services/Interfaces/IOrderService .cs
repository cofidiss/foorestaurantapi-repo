using FooRestaurantApi.ServicesLayer.DTO.Order;
using FooRestaurantApi.ServicesLayer.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooRestaurantApi.ServicesLayer.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ApiResponseDto<string, long?>> AddOrder(AddOrderRequestDto addOrderRequestDto);
        Task<ApiResponseDto<string, OrderDetailDto>> GetOrderDetail(long id);
        Task<ApiResponseDto<string, OrderDetailAdminDto>> GetOrderDetailAdmin(long id);
        Task UpdateOrderStatusAdmin(long id);
        Task<ApiResponseDto<string, bool>> IsOrderStatusUpdateValidForUser(long orderId, int statusId);
        Task<ApiResponseDto<string, bool>> IsOrderStatusUpdateValidForAdmin(long orderId, int statusId);

        Task<ApiResponseDto<string, long>> UpdateOrderStatus(long orderId, int statusId);
        Task<ApiResponseDto<string, long>> UpdateOrderStatusAdmin(long orderId, int statusId);
        Task<ApiResponseDto<string, IEnumerable<FilteredOrderDto>>> FilterOrder(FilterOrderRequestDto filterOrderDto);
        Task<ApiResponseDto<string, IEnumerable<FilteredOrderAdminDto>>> FilterOrderAdmin(FilterOrderRequestAdminDto filterOrderRequestAdminDto);
        ApiResponseDto<string, IEnumerable<StatusLovAdminDto>> GetStatusLovAdmin();
        ApiResponseDto<string, IEnumerable<StatusLovDto>> GetStatusLov();
    }
}
