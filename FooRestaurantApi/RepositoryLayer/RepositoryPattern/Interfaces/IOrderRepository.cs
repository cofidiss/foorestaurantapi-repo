using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Model.Order;
using FooRestaurantApi.DomainLayer.Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces
{

    public interface IOrderRepository : IRepository<OrderEntity>
    {
        Task<ApiResponseModel<string, long?>> AddOrder(AddOrderRequestModel addOrderRequestModel);
        Task<ApiResponseModel<string, OrderDetailModel>> GetOrderDetail(long id);
        Task<ApiResponseModel<string, OrderDetailAdminModel>> GetOrderDetailAdmin(long id);
        Task<ApiResponseModel<string, IEnumerable<FilteredOrderModel>>> FilterOrder(FilterOrderRequestModel filterOrderModel);
        Task<ApiResponseModel<string, IEnumerable<FilteredOrderAdminModel>>> FilterOrderAdmin(FilterOrderRequestAdminModel filterOrderRequestAdminModel);
    }
}
