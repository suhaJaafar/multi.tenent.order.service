using MultiTenantOrderService.Application.Abstractions.Messaging;
using MultiTenantOrderService.Domain.Identity.FiltersParams;

namespace MultiTenantOrderService.Application.GetUsers;

public record GetUsersQuery(UsersParams Parameters) : IQuery<UsersListResponse>;

