using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.Business;
using DeviceMonitoring.Services.DataTransferObjects.Measurments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.API.Controllers;

public class MeasurementsController : BaseController
{
    private readonly IMeasurementService _measurementService;
    private readonly ILogger<MeasurementsController> _logger;

    public MeasurementsController(IMeasurementService measurementService, ILogger<MeasurementsController> logger)
    {
        _measurementService = measurementService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Measurement>>> Get()
    {
        try
        {
            var measurements = await _measurementService.GetAllAsync();
            return Ok(measurements);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
            return BadRequest();
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<Measurement>> GetById(int id)
    {
        try
        {
            var measurement = await _measurementService.GetByIdAsync(id);
            return Ok(measurement);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create(MeasurementForInsertDto measurementForInsert)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            await _measurementService.AddAsync(measurementForInsert);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
            return BadRequest();
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> Update(int id, MeasurementForUpdateDto measurementForUpdate)
    {
        if (!ModelState.IsValid || id != measurementForUpdate.Id)
        {
            return BadRequest();
        }

        try
        {
            await _measurementService.UpdateAsync(measurementForUpdate);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _measurementService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
            return BadRequest();
        }
    }
}
