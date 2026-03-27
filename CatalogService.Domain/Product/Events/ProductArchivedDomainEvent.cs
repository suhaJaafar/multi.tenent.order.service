using CatalogService.Domain.Abstractions;

namespace CatalogService.Domain.Product.Events;

public sealed record ProductArchivedDomainEvent(Guid ProductId) : IDomainEvent;

