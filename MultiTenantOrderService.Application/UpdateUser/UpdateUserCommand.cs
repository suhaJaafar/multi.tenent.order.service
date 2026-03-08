using MultiTenantOrderService.Application.Abstractions.Messaging;
using MultiTenantOrderService.Application.GetUser;
using MultiTenantOrderService.Domain.Enums;
using MultiTenantOrderService.Domain.Identity.Enums;

namespace MultiTenantOrderService.Application.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string Name,
    string Email,
    string? Password,
    string? PhoneNumber,
    UserType UserType,
    TenentName TenentName) : ICommand<UserResponse>;

