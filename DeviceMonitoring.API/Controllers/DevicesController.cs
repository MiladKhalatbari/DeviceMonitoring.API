using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.Devices;
using DeviceMonitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.API.Controllers;

[Route("api/devices")]
public sealed class DevicesController(IDeviceService deviceService) : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Device>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Device>>> GetAllAsync()
    {
        var devices = await deviceService.GetAllAsync();

        return Ok(devices);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetDeviceById")]
    [ProducesResponseType(typeof(Device), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Device>> GetByIdAsync(int id)
    {
        var device = await deviceService.GetByIdAsync(id);

        return Ok(device);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] DeviceForInsertDto deviceForInsert)
    {
        var createdDeviceId = await deviceService.AddAsync(deviceForInsert);

        return CreatedAtRoute(
            "GetDeviceById",
            new { id = createdDeviceId },
            new { id = createdDeviceId });
    }

    [HttpPut("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] DeviceForUpdateDto deviceForUpdate)
    {
        if (id != deviceForUpdate.Id)
        {
            throw new ArgumentException(
                "The route id must match the device id in the request body.");
        }

        await deviceService.UpdateAsync(deviceForUpdate);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await deviceService.DeleteAsync(id);

        return NoContent();
    }
}