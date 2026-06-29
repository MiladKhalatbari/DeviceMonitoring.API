using System.ComponentModel.DataAnnotations;

namespace DeviceMonitoring.Services.DataTransferObjects.Devices;

public record DeviceForInsertDto
{

    [Required(ErrorMessage = "Device name is required.")]
    [StringLength(256, ErrorMessage = "Device name cannot exceed 256 characters.")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(256, ErrorMessage = "Location cannot exceed 256 characters.")]
    public string Location { get; init; } = string.Empty;

    [Required(ErrorMessage = "Manufacturer is required.")]
    [StringLength(256, ErrorMessage = "Manufacturer cannot exceed 256 characters.")]
    public string Manufacturer { get; init; } = string.Empty;

    [Required(ErrorMessage = "Model number is required.")]
    [StringLength(256, ErrorMessage = "Model number cannot exceed 256 characters.")]
    public string ModelNumber { get; init; } = string.Empty;
}
