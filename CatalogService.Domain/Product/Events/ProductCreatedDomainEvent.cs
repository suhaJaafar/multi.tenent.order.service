using CatalogService.Domain.Abstractions;
using CatalogService.Domain.Product.Enums;

namespace CatalogService.Domain.Product.Events;

public sealed record ProductCreatedDomainEvent(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid OwnerUserId,
    ProductCategory Category,
    bool IsActive) : IDomainEvent;
