namespace Shared.Contracts.Events;

/// <summary>
/// Integration event published when a product is created in the Catalog Service
/// </summary>
public record ProductCreatedIntegrationEvent
{
    public Guid ProductId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public Guid OwnerUserId { get; init; }
    public string Category { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid TenantId { get; init; } // For multi-tenant support
}
