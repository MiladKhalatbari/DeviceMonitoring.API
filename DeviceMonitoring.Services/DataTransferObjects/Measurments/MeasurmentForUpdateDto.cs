namespace DeviceMonitoring.Services.DataTransferObjects.Measurments;

public record MeasurementForUpdateDto
{
    public int Id { get; set; }

    public int DeviceId { get; set; }

    public double Value { get; set; }
}
