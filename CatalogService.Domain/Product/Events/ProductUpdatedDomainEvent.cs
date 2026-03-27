using CatalogService.Domain.Abstractions;
using CatalogService.Domain.Product.Enums;

namespace CatalogService.Domain.Product.Events;

public sealed record ProductUpdatedDomainEvent(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    ProductCategory Category) : IDomainEvent;


