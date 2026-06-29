namespace DeviceMonitoring.Domain.Entities;

public class Device
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Location { get; set; }

    public string? Manufacturer { get; set; }

    public string? ModelNumber { get; set; }

    public DateTime InstallationDate { get; set; }

    public bool IsActive { get; set; }

    public ICollection<Measurement> Measurements { get; set; } = [];
}
