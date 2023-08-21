using DeviceMonitoring.Services.Business;
using DeviceMonitoring.Services.DataTransferObjects.Devices;
using DeviceMonitoring.Services.DataTransferObjects.Measurments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.API.Controllers;


public class DataController : BaseController
{
    private readonly IDeviceService _deviceService;
    private readonly IMeasurementService _measurementService;
    private readonly ILogger<DataController> _logger;

    public DataController(IDeviceService deviceService, IMeasurementService measurementService, ILogger<DataController> logger)
    {
        _deviceService = deviceService;
        _measurementService = measurementService;
        _logger = logger;
    }

    [HttpPost("deviceName")]
    public async Task<ActionResult> Listen(string deviceName, double value)
    {
        try
        {
            var device = await _deviceService.GetByNameAsync(deviceName);
            var deviceId = device?.Id;
            if (deviceId is null)
            {
                deviceId = await _deviceService.AddAsync(new DeviceForInsertDto
                {
                    Name = deviceName,
                    Location = "Unknown",
                    Manufacturer = "Unknown",
                    ModelNumber = "Unknown",
                });
            }

            await _measurementService.AddAsync(new MeasurementForInsertDto
            {
                DeviceId = deviceId.Value,
                Value = value
            });

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
            return BadRequest();
        }
    }
}
