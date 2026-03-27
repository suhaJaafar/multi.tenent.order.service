namespace Shared.Contracts.Events;

/// <summary>
/// Integration event published when a product is archived/deleted in the Catalog Service
/// </summary>
public record ProductArchivedIntegrationEvent
{
    public Guid ProductId { get; init; }
    public DateTime ArchivedAt { get; init; }
}

