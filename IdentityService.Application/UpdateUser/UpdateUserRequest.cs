using IdentityService.Application.CreateUser;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Identity.Enums;

namespace IdentityService.Application.UpdateUser;

public record UpdateUserRequest(
    Guid Id,
    string Name,
    string Email,
    string? Password,
    UserType UserType,
    TenentName TenentName,
    string? PhoneNumber) : CreateUserRequest(Name, Email, Password ?? string.Empty, UserType, TenentName);

