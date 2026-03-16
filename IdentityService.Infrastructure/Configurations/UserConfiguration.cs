using IdentityService.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Name)
            .HasMaxLength(200);

        builder.Property(user => user.Email)
            .HasMaxLength(400)
            .HasConversion(email => email.Value, value => new Domain.Identity.ObjectValues.Email(value));

        builder.HasIndex(user => user.Email).IsUnique();
        
            builder.OwnsOne(user => user.Password, passwordBuilder =>
            {
                passwordBuilder.Property(password => password.Value)
                    .HasColumnName("Password")
                    .HasMaxLength(400);
            });
            
            builder.Property(user => user.TenentName)
                .HasConversion<string>()
                .HasMaxLength(50);
    

            builder.OwnsOne(user => user.PhoneNumber, phoneNumberBuilder =>
            {
                phoneNumberBuilder.Property(phoneNumber => phoneNumber.Value)
                    .HasColumnName("PhoneNumber")
                    .HasMaxLength(20);
            });
    
            builder.Property(user => user.UserType)
                .HasConversion<string>()
                .HasMaxLength(50);
    
            builder.Property<uint>("Version").IsRowVersion();
    }
}
