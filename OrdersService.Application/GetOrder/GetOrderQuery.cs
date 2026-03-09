using OrdersService.Application.Abstractions.Messaging;
using OrdersService.Application.CreateOrder;

namespace OrdersService.Application.GetOrder;

public record GetOrderQuery(Guid OrderId) : IQuery<OrderResponse>;

