using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CatalogService.Domain.DBContexts;

public class CatalogContext : DbContext
{
    private readonly IConfiguration _configuration;

    public CatalogContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public CatalogContext(DbContextOptions<CatalogContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Product.Entities.Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product.Entities.Product>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.Description)
                .HasMaxLength(2000);

            entity.Property(p => p.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(p => p.Stock)
                .IsRequired();

            entity.Property(p => p.OwnerUserId)
                .IsRequired();

            entity.Property(p => p.Category)
                .IsRequired();

            entity.Property(p => p.IsActive)
                .HasDefaultValue(true);

            // Index on OwnerUserId for efficient queries by owner
            entity.HasIndex(p => p.OwnerUserId);
        });
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

