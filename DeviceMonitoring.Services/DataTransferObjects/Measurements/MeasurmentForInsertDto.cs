using System.ComponentModel.DataAnnotations;

namespace DeviceMonitoring.Services.DataTransferObjects.Measurements;

public record MeasurementForInsertDto
{
    [Range(1, int.MaxValue)]
    public int DeviceId { get; init; }

    [Required]
    public double? Value { get; init; }
}