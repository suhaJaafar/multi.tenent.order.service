using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CatalogService.Infrastructure.DBContexts;

public class CatalogContextFactory : IDesignTimeDbContextFactory<CatalogContext>
{
    public CatalogContext CreateDbContext(string[] args)
    {
        var basePath = FindAppSettingsDirectory() ?? Path.Combine(Directory.GetCurrentDirectory(), "..", "CatalogService.Api");

        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection")
                               ?? Environment.GetEnvironmentVariable("CONNECTION_STRING")
                               ?? Environment.GetEnvironmentVariable("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException($"Connection string not found for design-time DbContext creation. Looked in: {basePath} and environment variables 'CONNECTION_STRING'/'DefaultConnection'.");

        var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>();
        optionsBuilder.UseNpgsql(connectionString);

        // Design-time context creation - IPublisher is not needed for migrations
        return new CatalogContext(optionsBuilder.Options, null!);
    }

    private static string? FindAppSettingsDirectory()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        for (var i = 0; i < 8 && dir != null; i++)
        {
            var candidate = Path.Combine(dir.FullName, "appsettings.json");
            if (File.Exists(candidate))
                return dir.FullName;
            dir = dir.Parent;
        }
        return null;
    }
}


