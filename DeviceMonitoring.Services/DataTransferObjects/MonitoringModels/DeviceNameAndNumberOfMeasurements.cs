namespace DeviceMonitoring.Services.DataTransferObjects.MonitoringModels;
public record DeviceNameAndNumberOfMeasurements
{
   public required string DeviceName { get; set; }
   public int NumberOfMeasurements { get; set; }
}
