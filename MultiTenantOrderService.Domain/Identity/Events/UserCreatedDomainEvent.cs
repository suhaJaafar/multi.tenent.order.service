using MultiTenantOrderService.Domain.Abstractions;

namespace MultiTenantOrderService.Domain.Identity.Events;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;