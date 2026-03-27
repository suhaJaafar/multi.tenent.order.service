namespace Shared.Contracts.Events;

/// <summary>
/// Integration event published when a product price changes in the Catalog Service
/// </summary>
public record ProductPriceChangedIntegrationEvent
{
    public Guid ProductId { get; init; }
    public decimal OldPrice { get; init; }
    public decimal NewPrice { get; init; }
    public DateTime ChangedAt { get; init; }
}

