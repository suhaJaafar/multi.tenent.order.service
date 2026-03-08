using MultiTenantOrderService.Domain.Enums;
using MultiTenantOrderService.Domain.Identity.Enums;

namespace MultiTenantOrderService.Application.CreateUser;

public record CreateUserRequest(
    string Name,
    string Email,
    string Password,
    UserType UserType,
    TenentName TenentName);