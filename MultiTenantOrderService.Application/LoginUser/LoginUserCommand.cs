using MultiTenantOrderService.Application.Abstractions.Messaging;

namespace MultiTenantOrderService.Application.LoginUser;

public record LoginUserCommand(
    string Email,
    string Password) : ICommand<LoginResponse>;

