using System.ComponentModel.DataAnnotations;

namespace DeviceMonitoring.Services.DataTransferObjects.Measurements;

public class MeasurementIngestionRequestDto
{
    [Required(ErrorMessage = "Device name is required.")]
    [StringLength(100, ErrorMessage = "Device name cannot exceed 100 characters.")]
    public string DeviceName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Measurement value is required.")]
    public double? Value { get; init; }
}