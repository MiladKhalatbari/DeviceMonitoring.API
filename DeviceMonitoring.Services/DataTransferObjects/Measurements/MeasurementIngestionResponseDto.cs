namespace DeviceMonitoring.Services.DataTransferObjects.Measurements;

public record MeasurementIngestionResponseDto(
    int DeviceId,
    bool DeviceCreated);