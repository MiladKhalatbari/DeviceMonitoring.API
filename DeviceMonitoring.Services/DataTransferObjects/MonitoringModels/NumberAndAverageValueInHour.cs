namespace DeviceMonitoring.Services.DataTransferObjects.MonitoringModels;
public record NumberAndAverageValueInHour
{
    public int AtHour { get; set; }
    public int NumberOfMeasurements { get; set; }
    public double? AverageValue { get; set; }
}
