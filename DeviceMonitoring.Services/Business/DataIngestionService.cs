using DeviceMonitoring.Services.DataTransferObjects.Devices;
using DeviceMonitoring.Services.DataTransferObjects.Measurements;
using DeviceMonitoring.Services.Interfaces;

namespace DeviceMonitoring.Services.Business;

public class DataIngestionService(IDeviceService deviceService, IMeasurementService measurementService) : IDataIngestionService
{
    private const string UNKNOWN_VALUE = "Unknown";

    public async Task<MeasurementIngestionResponseDto> IngestAsync(MeasurementIngestionRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deviceName = request.DeviceName?.Trim();

        if (string.IsNullOrWhiteSpace(deviceName))
        {
            throw new ArgumentException("Device name is required.", nameof(request.DeviceName));
        }

        if (request.Value is null)
        {
            throw new ArgumentException("Measurement value is required.", nameof(request.Value));
        }

        var measurementValue = request.Value.Value;

        if (double.IsNaN(measurementValue) || double.IsInfinity(measurementValue))
        {
            throw new ArgumentException("Measurement value must be a valid finite number.", nameof(request.Value));
        }

        var device = await deviceService.GetByNameAsync(deviceName);

        var deviceCreated = device is null;
        int deviceId;

        if (deviceCreated)
        {
            deviceId = await deviceService.AddAsync(
                new DeviceForInsertDto
                {
                    Name = deviceName,
                    Location = UNKNOWN_VALUE,
                    Manufacturer = UNKNOWN_VALUE,
                    ModelNumber = UNKNOWN_VALUE
                });
        }
        else
        {
            deviceId = device!.Id;
        }

        await measurementService.AddAsync(
            new MeasurementForInsertDto
            {
                DeviceId = deviceId,
                Value = measurementValue
            });

        return new MeasurementIngestionResponseDto(
            DeviceId: deviceId,
            DeviceCreated: deviceCreated);
    }
}
