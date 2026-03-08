using MultiTenantOrderService.Application.CreateUser;
using MultiTenantOrderService.Domain.Enums;
using MultiTenantOrderService.Domain.Identity.Enums;

namespace MultiTenantOrderService.Application.UpdateUser;

public record UpdateUserRequest(
    Guid Id,
    string Name,
    string Email,
    string? Password,
    UserType UserType,
    TenentName TenentName,
    string? PhoneNumber) : CreateUserRequest(Name, Email, Password ?? string.Empty, UserType, TenentName);

