using IdentityService.Domain.Abstractions;

namespace IdentityService.Domain.Identity.Events;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;