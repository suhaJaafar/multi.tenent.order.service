namespace OrdersService.Application.CreateOrder;

public record CreateOrderRequest(
    List<OrderItemRequest> Items,
    string? Notes
);
