namespace IdentityService.Application.LoginUser;

public sealed class LoginResponse
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string UserType { get; init; } = string.Empty;
    public string TenentName { get; init; } = string.Empty;
}

