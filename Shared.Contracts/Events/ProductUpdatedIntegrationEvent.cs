namespace Shared.Contracts.Events;

/// <summary>
/// Integration event published when a product is updated in the Catalog Service
/// </summary>
public record ProductUpdatedIntegrationEvent
{
    public Guid ProductId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public string Category { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime UpdatedAt { get; init; }
}

