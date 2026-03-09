using OrdersService.Domain.Abstractions;

namespace OrdersService.Domain.Order.Events;

public sealed record OrderCreatedDomainEvent(Guid OrderId, Guid UserId, decimal TotalAmount) : IDomainEvent;

