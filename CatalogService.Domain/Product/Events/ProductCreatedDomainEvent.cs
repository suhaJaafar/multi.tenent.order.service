using CatalogService.Domain.Abstractions;

namespace CatalogService.Domain.Product.Events;

public sealed record ProductCreatedDomainEvent(Guid ProductId) : IDomainEvent;