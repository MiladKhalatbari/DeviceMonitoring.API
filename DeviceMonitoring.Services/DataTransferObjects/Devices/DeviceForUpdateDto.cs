using System.ComponentModel.DataAnnotations;

namespace DeviceMonitoring.Services.DataTransferObjects.Devices;

public record DeviceForUpdateDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Device ID must be greater than zero.")]
    public int Id { get; init; }

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(256, ErrorMessage = "Location cannot exceed 256 characters.")]
    public string Location { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}
