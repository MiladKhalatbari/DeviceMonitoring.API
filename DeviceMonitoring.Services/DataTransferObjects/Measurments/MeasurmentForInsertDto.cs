namespace DeviceMonitoring.Services.DataTransferObjects.Measurments;

public record MeasurementForInsertDto
{
    public int DeviceId { get; set; }

    public double Value { get; set; }
}
