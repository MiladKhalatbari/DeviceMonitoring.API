using DeviceMonitoring.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceMonitoring.Data.Configurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(user => user.UserName)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(user => user.UserName)
            .IsUnique();

        builder.Property(user => user.PasswordHash)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(user => user.FirstName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(user => user.LastName)
            .HasMaxLength(256)
            .IsRequired();
    }
}