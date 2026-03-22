using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersService.Application.Abstractions.Data;
using OrdersService.Domain.Abstractions;
using OrdersService.Domain.DBContexts;
using OrdersService.Domain.Order;
using OrdersService.Infrastructure.Data;
using OrdersService.Infrastructure.Repositories;

namespace OrdersService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        
        var connectionString =
            configuration.GetConnectionString("DefaultConnection") ??
            throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");

        // Register OrdersContext
        services.AddDbContext<OrdersContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString);
        });
        
        // Register DbContext as the implementation for DbContext dependency in repositories
        services.AddScoped<DbContext>(provider =>
            provider.GetRequiredService<OrdersContext>());

        // Register repositories and UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        
        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(connectionString));

        return services;
    }
}