using System.ComponentModel.DataAnnotations;

namespace DeviceMonitoring.Services.DataTransferObjects.Devices;

public record DeviceForInsertDto
{

    [MaxLength(256)]
    public string Name { get; set; }

    [MaxLength(256)]
    public string Location { get; set; }

    [MaxLength(256)]
    public string Manufacturer { get; set; }

    [MaxLength(256)]
    public string ModelNumber { get; set; }
}
