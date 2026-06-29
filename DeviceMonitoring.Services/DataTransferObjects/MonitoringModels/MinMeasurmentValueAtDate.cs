namespace DeviceMonitoring.Services.DataTransferObjects.MonitoringModels;
public record MinMeasurmentValueAtDate
{
    public double? MinValue { get; set; }
    public DateTime AtDate { get; set; }
}
