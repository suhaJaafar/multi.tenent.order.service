namespace OrdersService.Application.CreateOrder;

public sealed class OrderResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public decimal TotalAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public DateTime CreateAt { get; init; }
    public DateTime? UpdateAt { get; init; }
}

