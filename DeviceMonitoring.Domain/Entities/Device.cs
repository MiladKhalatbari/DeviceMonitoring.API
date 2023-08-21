using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceMonitoring.Domain.Entities;

public record Device
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Location { get; set; }

    public string Manufacturer { get; set; }

    public string ModelNumber { get; set; }

    public DateTime InstallationDate { get; set; }

    public bool IsActive { get; set; }

    public ICollection<Measurement> Measurements { get; set; }
}

public class DeviceConfig : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
        builder.Property(p => p.Location).HasMaxLength(256).IsRequired(); ;
        builder.Property(p => p.Manufacturer).HasMaxLength(256);
        builder.Property(p => p.ModelNumber).HasMaxLength(256);
        builder.HasIndex(p => new { p.Name }).IsUnique();
    }
}
