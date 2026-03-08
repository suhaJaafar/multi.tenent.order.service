using MultiTenantOrderService.Application.Abstractions.Messaging;

namespace MultiTenantOrderService.Application.GetUser;

public record GetUserQuery(Guid UserId) : IQuery<UserResponse>;

