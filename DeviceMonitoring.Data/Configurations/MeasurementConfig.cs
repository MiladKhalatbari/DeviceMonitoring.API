using DeviceMonitoring.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceMonitoring.Data.Configurations;

public class MeasurementConfig : IEntityTypeConfiguration<Measurement>
{
    public void Configure(EntityTypeBuilder<Measurement> builder)
    {
        builder.ToTable("Measurements");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Value)
            .IsRequired();

        builder.Property(p => p.CreatedOn)
            .IsRequired();

        builder.HasIndex(p => p.DeviceId);

        builder.HasOne(p => p.Device)
            .WithMany(d => d.Measurements)
            .HasForeignKey(p => p.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
