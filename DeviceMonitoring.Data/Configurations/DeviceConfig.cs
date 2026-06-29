using DeviceMonitoring.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceMonitoring.Data.Configurations;

public class DeviceConfig : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("Devices");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(p => p.Location)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(p => p.Manufacturer)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(p => p.ModelNumber)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder.HasMany(p => p.Measurements)
            .WithOne(m => m.Device)
            .HasForeignKey(m => m.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}