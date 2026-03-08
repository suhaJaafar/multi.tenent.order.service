using Microsoft.EntityFrameworkCore;
using MultiTenantOrderService.Application.Abstractions.Messaging;
using MultiTenantOrderService.Domain.Abstractions;
using MultiTenantOrderService.Domain.DBContexts;
using MultiTenantOrderService.Domain.Identity;
using Scrypt;

namespace MultiTenantOrderService.Application.LoginUser;

internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginResponse>
{
    private readonly OSContext _context;

    public LoginUserCommandHandler(OSContext context)
    {
        _context = context;
    }

    public async Task<Result<LoginResponse>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        // Find user by email
        var user = await _context.Users
            .Where(x => x.Email.Value == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
        }

        // Verify password
        var scryptEncoder = new ScryptEncoder();
        if (!scryptEncoder.Compare(request.Password, user.Password.Value))
        {
            return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
        }

        // Create login response
        var response = new LoginResponse
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email.Value,
            UserType = user.UserType.ToString(),
            TenentName = user.TenentName.ToString()
        };

        return Result.Success(response);
    }
}

