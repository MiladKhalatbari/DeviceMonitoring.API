using System.ComponentModel.DataAnnotations;

namespace DeviceMonitoring.Services.DataTransferObjects.Devices;

public record DeviceForUpdateDto
{
    public int Id { get; set; }

    [MaxLength(256)]
    public string Location { get; set; }

    public bool IsActive { get; set; }
}
