using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantOrderService.Domain.Abstractions;
using MultiTenantOrderService.Domain.DBContexts;
using MultiTenantOrderService.Domain.Identity;
using MultiTenantOrderService.Infrastructure.Repositories;

namespace MultiTenantOrderService.Infrastructure;

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

