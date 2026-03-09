using OrdersService.Application.Abstractions.Messaging;

namespace OrdersService.Application.CreateOrder;

public record CreateOrderCommand(
    Guid Id,
    Guid UserId,
    decimal TotalAmount,
    string? Notes) : ICommand<OrderResponse>;

