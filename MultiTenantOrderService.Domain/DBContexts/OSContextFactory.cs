using System;
using System.IO;
using MultiTenantOrderService.Domain.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


namespace MultiTenantOrderService.Domain.DBContexts
{
    public class OSContextFactory : IDesignTimeDbContextFactory<OSContext>
    {
        public OSContext CreateDbContext(string[] args)
        {
            var basePath = FindAppSettingsDirectory() ?? Path.Combine(Directory.GetCurrentDirectory(), "..", "MultiTenantOrderMultiTenantOrderService.Api");

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

            var optionsBuilder = new DbContextOptionsBuilder<OSContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new OSContext(optionsBuilder.Options, config);
        }

        private static string FindAppSettingsDirectory()
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
}
