using IdentityService.Domain.Enums;
using IdentityService.Domain.Identity.Enums;

namespace IdentityService.Application.CreateUser;

public record CreateUserRequest(
    string Name,
    string Email,
    string Password,
    UserType UserType,
    TenentName TenentName);