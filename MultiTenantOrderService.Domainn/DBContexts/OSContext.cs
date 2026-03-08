using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MultiTenantOrderService.Domain.Entities;

namespace MultiTenantOrderService.Domain.DBContexts
{
    public class OSContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public OSContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        //Constructor with DbContextOptions and the context class itself
        public OSContext(DbContextOptions<OSContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var c = entityType.FindProperty("IsDeleted");
                if (c != null)
                {
                    var parameter = Expression.Parameter(entityType.ClrType);
                    var propertyMethodInfo = typeof(EF).GetMethod("Property")!.MakeGenericMethod(typeof(bool));
                    var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));
                    BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));

                    var lambda = Expression.Lambda(compareExpression, parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }

            }

            // Configure Email and Password as owned types (value objects)
            modelBuilder.Entity<User>()
                .OwnsOne(u => u.Email, email =>
                {
                    email.Property(e => e.Value)
                        .HasColumnName("Email")
                        .IsRequired(false);
                    email.WithOwner();
                });
            
            modelBuilder.Entity<User>()
                .OwnsOne(u => u.Password, password =>
                {
                    password.Property(p => p.Value)
                        .HasColumnName("Password")
                        .IsRequired();
                    password.WithOwner();
                });
            modelBuilder.Entity<User>()
                .OwnsOne(u => u.PhoneNumber, phoneNumber =>
                {
                    phoneNumber.Property(p => p.Value)
                        .HasColumnName("PhoneNumber")
                        .IsRequired(false);
                    phoneNumber.WithOwner();
                });

            // // Configure OrderStatus default value to Pending for new rows and when adding the column
            // modelBuilder.Entity<Order>()
            //     .Property(o => o.OrderStatus)
            //     .HasDefaultValue(Enums.OrderStatus.Pending);

            // Many-to-many: Order <-> Product via join table OrderProducts
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithMany(p => p.Orders)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderProducts",
                    j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Order>().WithMany().HasForeignKey("OrderId").OnDelete(DeleteBehavior.Cascade)
                );
        }


    }
}
