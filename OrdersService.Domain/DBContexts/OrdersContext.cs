using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrdersService.Domain.Order.Entities;

namespace OrdersService.Domain.DBContexts;

public class OrdersContext : DbContext
{
    private readonly IConfiguration? _configuration;

    public OrdersContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public OrdersContext(DbContextOptions<OrdersContext> options, IConfiguration? configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Order.Entities.Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Order entity
        modelBuilder.Entity<Order.Entities.Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            
            entity.Property(o => o.UserId)
                .IsRequired();
            
            entity.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            entity.Property(o => o.Status)
                .IsRequired();
            
            entity.Property(o => o.Notes)
                .HasMaxLength(1000);

            entity.Property(o => o.CreateAt)
                .IsRequired();

            // Add index on UserId for faster queries
            entity.HasIndex(o => o.UserId);
            
            // Add index on Status for filtering
            entity.HasIndex(o => o.Status);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && _configuration != null)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertDateTimesToUtc();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ConvertDateTimesToUtc()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            foreach (var property in entry.Properties)
            {
                if (property.CurrentValue is DateTime dateTime)
                {
                    if (dateTime.Kind == DateTimeKind.Local)
                    {
                        property.CurrentValue = dateTime.ToUniversalTime();
                    }
                    else if (dateTime.Kind == DateTimeKind.Unspecified)
                    {
                        property.CurrentValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    }
                }
            }
        }
    }
}

