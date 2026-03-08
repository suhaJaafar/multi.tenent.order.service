using MultiTenantOrderService.Application.Abstractions.Messaging;
using MultiTenantOrderService.Application.GetUser;
using MultiTenantOrderService.Domain.Enums;
using MultiTenantOrderService.Domain.Identity.Enums;

namespace MultiTenantOrderService.Application.CreateUser;

public record CreateUserCommand(
    Guid Id,
    string Name,
    string Email,
    string Password,
    UserType UserType,
    TenentName TenentName) : ICommand<UserResponse>;




