using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Domain.Identity.FiltersParams;

namespace IdentityService.Application.GetUsers;

public record GetUsersQuery(UsersParams Parameters) : IQuery<UsersListResponse>;

