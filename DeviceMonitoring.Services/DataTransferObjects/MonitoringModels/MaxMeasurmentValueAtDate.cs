namespace DeviceMonitoring.Services.DataTransferObjects.MonitoringModels;
public record MaxMeasurmentValueAtDate
{
    public double? MaxValue { get; set; }
    public DateTime AtDate { get; set; }
}