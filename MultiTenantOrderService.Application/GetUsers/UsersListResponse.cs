using MultiTenantOrderService.Domain.Identity.DTOs;

namespace MultiTenantOrderService.Application.GetUsers;

public sealed class UsersListResponse
{
    public List<UsersToReturnDto> Users { get; init; } = new();
    public int TotalCount { get; init; }
}

