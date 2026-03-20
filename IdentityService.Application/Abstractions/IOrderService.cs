using IdentityService.Domain.DTOs;
using IdentityService.Domain.FiltersParams;
using IdentityService.Domain.Forms;
using IdentityService.Domain.Identity.DTOs;

namespace IdentityService.Application.Abstractions;

public interface IOrderService
{
    Task<ServiceResponse<List<OrderToReturn>>> GetOrdersWithProducts(OrdersParams orderParams, ActiveUserData activeUserData);
    Task<ServiceResponse<OrderDto>> AddOrder(CreateOrderForm createForm, ActiveUserData activeUserData);
    Task<ServiceResponse<OrderDto>> UpdateOrder(Guid id, UpdateOrderForm updateForm, ActiveUserData activeUserData);
    Task<ServiceResponse<bool>> DeleteOrder(Guid id);
}

