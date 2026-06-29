using System.ComponentModel.DataAnnotations;

namespace DeviceMonitoring.Services.DataTransferObjects.Measurements;

public record MeasurementForUpdateDto
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public double? Value { get; init; }
}
