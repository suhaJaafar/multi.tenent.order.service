using CatalogService.Domain.Abstractions;

namespace CatalogService.Domain.Product.Events;

public sealed record ProductPriceChangedDomainEvent(
    Guid ProductId,
    decimal OldPrice,
    decimal NewPrice) : IDomainEvent;

