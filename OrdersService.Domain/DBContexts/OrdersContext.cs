using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrdersService.Domain.Order.Entities;
using OrdersService.Domain.Product.Entities;

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
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<ProductReference> ProductReferences { get; set; }

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

            // Configure relationship with OrderItems
            entity.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add index on UserId for faster queries
            entity.HasIndex(o => o.UserId);
            
            // Add index on Status for filtering
            entity.HasIndex(o => o.Status);
        });

        // Configure OrderItem entity
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(oi => oi.Id);

            entity.Property(oi => oi.ProductId)
                .IsRequired();

            entity.Property(oi => oi.SKU)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(oi => oi.Quantity)
                .IsRequired();

            entity.Property(oi => oi.Subtotal)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Indexes for performance
            entity.HasIndex(oi => oi.OrderId);
            entity.HasIndex(oi => oi.ProductId);
        });

        // Configure ProductReference entity (local cache)
        modelBuilder.Entity<ProductReference>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.SKU)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.CurrentPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(p => p.Category)
                .HasMaxLength(50);

            entity.Property(p => p.IsActive)
                .IsRequired();

            entity.Property(p => p.LastSyncedAt)
                .IsRequired();

            // Indexes for performance
            entity.HasIndex(p => p.SKU);
            entity.HasIndex(p => p.IsActive);
            entity.HasIndex(p => p.OwnerUserId);
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

