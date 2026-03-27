namespace Shared.Contracts.Events;

/// <summary>
/// Integration event published when an order is created in the Orders Service
/// </summary>
public record OrderCreatedIntegrationEvent
{
    public Guid OrderId { get; init; }
    public Guid UserId { get; init; }
    public decimal TotalAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public List<OrderLineItem> OrderLines { get; init; } = new();
}

public record OrderLineItem
{
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public string SKU { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public decimal Subtotal { get; init; }
}

