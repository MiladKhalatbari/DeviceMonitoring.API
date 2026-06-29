using DeviceMonitoring.Services.DataTransferObjects.Measurements;
using DeviceMonitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.API.Controllers;

[Route("api/data")]
public class DataController(IDataIngestionService dataIngestionService) : BaseController
{
    [HttpPost("Ingest")]
    [ProducesResponseType(typeof(MeasurementIngestionResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MeasurementIngestionResponseDto>> IngestAsync([FromBody] MeasurementIngestionRequestDto request)
    {
        var response = await dataIngestionService.IngestAsync(request);

        return StatusCode(StatusCodes.Status201Created, response);
    }
}