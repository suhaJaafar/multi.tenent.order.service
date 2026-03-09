using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrdersService.Domain.DBContexts;

public class OrdersContextFactory : IDesignTimeDbContextFactory<OrdersContext>
{
    public OrdersContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdersContext>();
        
        // Use a default connection string for design-time migrations
        optionsBuilder.UseNpgsql("Host=localhost;Database=OrdersServiceDb;Username=postgres;Password=postgres");

        // For design-time, we pass null for IConfiguration
        // The DbContext will use OnConfiguring instead
        return new OrdersContext(optionsBuilder.Options, null!);
    }
}

