namespace IdentityService.Application.GetUser;

public sealed class UserResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string UserType { get; init; } = string.Empty;
    public string TenentName { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
}


