using DeviceMonitoring.Services.DataTransferObjects.Measurements;

namespace DeviceMonitoring.Services.Interfaces;

public interface IDataIngestionService
{
    Task<MeasurementIngestionResponseDto> IngestAsync(MeasurementIngestionRequestDto request);
}
