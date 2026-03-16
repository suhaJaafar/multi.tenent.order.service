﻿using CatalogService.Application.DeleteProduct;
 using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CatalogService.Domain.Abstractions;
using CatalogService.Domain.Product;
using CatalogService.Infrastructure.Data;
using CatalogService.Infrastructure.DBContexts;
using CatalogService.Infrastructure.Repositories;
using Dapper;

namespace CatalogService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection") ??
            throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");

        // Register CatalogContext
        services.AddDbContext<CatalogContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString);
        });
        
        // Register DbContext as the implementation for DbContext dependency in repositories
        services.AddScoped<DbContext>(provider =>
            provider.GetRequiredService<CatalogContext>());
        
        // Register repositories and UnitOfWork
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductQueryService, ProductQueryService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(connectionString));

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        return services;
    }
}