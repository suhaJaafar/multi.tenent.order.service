using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CatalogService.Domain.Abstractions;
using CatalogService.Domain.DBContexts;
using CatalogService.Domain.Product;
using CatalogService.Infrastructure.Repositories;

namespace CatalogService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register DbContext as the implementation for DbContext dependency in repositories
        services.AddScoped<DbContext>(provider =>
            provider.GetRequiredService<CatalogContext>());

        // Register repositories and UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}