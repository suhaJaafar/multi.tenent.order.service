using IdentityService.Domain.Identity.DTOs;

namespace IdentityService.Application.GetUsers;

public sealed class UsersListResponse
{
    public List<UsersToReturnDto> Users { get; init; } = new();
    public int TotalCount { get; init; }
}

