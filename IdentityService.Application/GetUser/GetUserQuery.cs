using IdentityService.Application.Abstractions.Messaging;

namespace IdentityService.Application.GetUser;

public record GetUserQuery(Guid UserId) : IQuery<UserResponse>;

