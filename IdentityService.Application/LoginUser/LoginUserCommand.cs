using IdentityService.Application.Abstractions.Messaging;

namespace IdentityService.Application.LoginUser;

public record LoginUserCommand(
    string Email,
    string Password) : ICommand<LoginResponse>;

