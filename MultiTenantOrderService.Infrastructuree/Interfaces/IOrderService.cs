using MultiTenantOrderService.Domain.DTOs;
using MultiTenantOrderService.Domain.FiltersParams;
using MultiTenantOrderService.Domain.Forms;
using MultiTenantOrderService.Infrastructure.Utilities;

namespace  MultiTenantOrderService.Infrastructure.Interfaces
{
    
}
public interface IOrderService
{
    Task<ServiceResponse<List<OrderToReturn>>> GetOrdersWithProducts(OrdersParams orderParams, ActiveUserData activeUserData);
    Task<ServiceResponse<OrderDto>> AddOrder(CreateOrderForm createForm, ActiveUserData activeUserData);
    Task<ServiceResponse<OrderDto>> UpdateOrder(Guid id, UpdateOrderForm updateForm, ActiveUserData activeUserData);
    Task<ServiceResponse<bool>> DeleteOrder(Guid id);
}