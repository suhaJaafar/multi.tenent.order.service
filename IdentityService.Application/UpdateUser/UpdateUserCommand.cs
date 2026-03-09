using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.GetUser;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Identity.Enums;

namespace IdentityService.Application.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string Name,
    string Email,
    string? Password,
    string? PhoneNumber,
    UserType UserType,
    TenentName TenentName) : ICommand<UserResponse>;

