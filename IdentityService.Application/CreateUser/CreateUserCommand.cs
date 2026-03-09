using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.GetUser;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Identity.Enums;

namespace IdentityService.Application.CreateUser;

public record CreateUserCommand(
    Guid Id,
    string Name,
    string Email,
    string Password,
    UserType UserType,
    TenentName TenentName) : ICommand<UserResponse>;




