using OrdersService.Application.Abstractions.Messaging;

namespace OrdersService.Application.CreateOrder;

public record CreateOrderCommand(
    Guid Id,
    Guid UserId,
    List<OrderItemRequest> Items,
    string? Notes) : ICommand<OrderResponse>;

public record OrderItemRequest(
    Guid ProductId,
    int Quantity);
