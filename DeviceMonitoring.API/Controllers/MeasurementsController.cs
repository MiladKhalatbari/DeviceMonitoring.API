using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.Measurements;
using DeviceMonitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.API.Controllers;

[Route("api/measurements")]
public class MeasurementsController(IMeasurementService measurementService) : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Measurement>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Measurement>>> GetAllAsync()
    {
        var measurements = await measurementService.GetAllAsync();

        return Ok(measurements);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetMeasurementById")]
    [ProducesResponseType(typeof(Measurement), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Measurement>> GetByIdAsync(int id)
    {
        var measurement = await measurementService.GetByIdAsync(id);

        return Ok(measurement);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] MeasurementForInsertDto measurementForInsert)
    {
        var createdMeasurementId = await measurementService.AddAsync(
            measurementForInsert);

        return CreatedAtRoute(
            "GetMeasurementById",
            new { id = createdMeasurementId },
            new { id = createdMeasurementId });
    }

    [HttpPut("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] MeasurementForUpdateDto measurementForUpdate)
    {
        if (id != measurementForUpdate.Id)
        {
            throw new ArgumentException(
                "The route id must match the measurement id in the request body.");
        }

        await measurementService.UpdateAsync(measurementForUpdate);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await measurementService.DeleteAsync(id);

        return NoContent();
    }
}