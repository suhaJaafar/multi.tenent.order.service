using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersService.Application.Abstractions.Data;
using OrdersService.Domain.Abstractions;
using OrdersService.Domain.DBContexts;
using OrdersService.Domain.Order;
using OrdersService.Domain.Product;
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

        // Register OrdersContext with connection resilience
        services.AddDbContext<OrdersContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly("OrdersService.Domain");
                // Enable connection resilience with retry on transient failures
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            });
        });
        
        // Register DbContext as the implementation for DbContext dependency in repositories
        services.AddScoped<DbContext>(provider =>
            provider.GetRequiredService<OrdersContext>());

        // Register repositories and UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductReferenceRepository, ProductReferenceRepository>();
        
        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(connectionString));

        return services;
    }
}