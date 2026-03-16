using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersService.Domain.Order.Entities;

namespace OrdersService.Infrastructure.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(order => order.Id);

        builder.Property(order => order.TotalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(order => order.Status)
            .HasConversion<string>()
            .HasMaxLength(50);
        
        builder.Property<uint>("Version").IsRowVersion();
    }
    
}