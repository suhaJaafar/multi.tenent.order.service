using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IdentityService.Domain.Abstractions;
using IdentityService.Domain.DBContexts;
using IdentityService.Domain.Identity;
using IdentityService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register DbContext as the implementation for DbContext dependency in repositories
        services.AddScoped<DbContext>(provider => 
            provider.GetRequiredService<OSContext>());

        // Register repositories and UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}

