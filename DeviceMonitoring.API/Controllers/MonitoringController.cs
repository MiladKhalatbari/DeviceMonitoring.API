using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.MonitoringModels;
using DeviceMonitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.API.Controllers;

[Route("api/monitoring/devices/{id:int:min(1)}")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
public sealed class MonitoringController(IMonitoringService monitoringService) : BaseController
{
    [HttpGet("total-measurement-count")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetTotalMeasurementCountAsync(int id)
    {
        var count = await monitoringService.GetTotalMeasurementCount(id);
        return Ok(count);
    }

    [HttpGet("summary")]
    [ProducesResponseType(typeof(DeviceNameAndNumberOfMeasurements), StatusCodes.Status200OK)]
    public async Task<ActionResult<DeviceNameAndNumberOfMeasurements>> GetDeviceSummaryAsync(int id)
    {
        var summary = await monitoringService.GetDeviceNameAndNumberOfMeasurements(id);
        return Ok(summary);
    }

    [HttpGet("minimum-values-by-date")]
    [ProducesResponseType(typeof(IEnumerable<MinMeasurmentValueAtDate>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MinMeasurmentValueAtDate>>> GetMinimumValuesByDateAsync(int id)
    {
        var values = await monitoringService.CalculateMinValue(id);
        return Ok(values);
    }

    [HttpGet("maximum-values-by-date")]
    [ProducesResponseType(typeof(IEnumerable<MaxMeasurmentValueAtDate>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MaxMeasurmentValueAtDate>>> GetMaximumValuesByDateAsync(int id)
    {
        var values = await monitoringService.CalculateMaxValue(id);
        return Ok(values);
    }

    [HttpGet("average-value")]
    [ProducesResponseType(typeof(double?), StatusCodes.Status200OK)]
    public async Task<ActionResult<double?>> GetAverageValueAsync(int id)
    {
        var averageValue = await monitoringService.CalculateAverageValue(id);
        return Ok(averageValue);
    }

    [HttpGet("measurement-count/above-threshold")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetMeasurementCountAboveThresholdAsync(int id, [FromQuery] double threshold)
    {
        var count = await monitoringService.GetMeasurementCountAboveThreshold(id, threshold);
        return Ok(count);
    }

    [HttpGet("measurement-count/below-threshold")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetMeasurementCountBelowThresholdAsync(int id, [FromQuery] double threshold)
    {
        var count = await monitoringService.GetMeasurementCountBelowThreshold(id, threshold);
        return Ok(count);
    }

    [HttpGet("average-value/date-range")]
    [ProducesResponseType(typeof(double?), StatusCodes.Status200OK)]
    public async Task<ActionResult<double?>> GetAverageValueForDateRangeAsync(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var averageValue = await monitoringService.CalculateAverageValueForDateRange(id, startDate, endDate);
        return Ok(averageValue);
    }

    [HttpGet("minimum-value/date-range")]
    [ProducesResponseType(typeof(double?), StatusCodes.Status200OK)]
    public async Task<ActionResult<double?>> GetMinimumValueForDateRangeAsync(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var minimumValue = await monitoringService.CalculateMinValueForDateRange(id, startDate, endDate);
        return Ok(minimumValue);
    }

    [HttpGet("maximum-value/date-range")]
    [ProducesResponseType(typeof(double?), StatusCodes.Status200OK)]
    public async Task<ActionResult<double?>> GetMaximumValueForDateRangeAsync(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var maximumValue = await monitoringService.CalculateMaxValueForDateRange(id, startDate, endDate);
        return Ok(maximumValue);
    }

    [HttpGet("measurements/date-range")]
    [ProducesResponseType(typeof(IEnumerable<Measurement>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurementsForDateRangeAsync(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var measurements = await monitoringService.GetMeasurementsForDateRange(id, startDate, endDate);
        return Ok(measurements);
    }

    [HttpGet("latest-measurements")]
    [ProducesResponseType(typeof(IEnumerable<Measurement>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Measurement>>> GetLatestMeasurementsAsync(int id, [FromQuery] int count)
    {
        var measurements = await monitoringService.GetLatestMeasurements(id, count);
        return Ok(measurements);
    }

    [HttpGet("hourly-summary")]
    [ProducesResponseType(typeof(IEnumerable<NumberAndAverageValueInHour>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<NumberAndAverageValueInHour>>> GetHourlySummaryAsync(int id, [FromQuery] DateTime date)
    {
        var summary = await monitoringService.GetNumberAndAverageValueInHourByDate(id, date);
        return Ok(summary);
    }

}