using OrdersService.Application.Abstractions.Messaging;
using OrdersService.Application.CreateOrder;

namespace OrdersService.Application.GetUserOrders;

public record GetUserOrdersQuery(Guid UserId) : IQuery<List<OrderResponse>>;

