using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.Business;
using DeviceMonitoring.Services.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DeviceMonitoring.API.Controllers
{

    [Route("Api/Monitoring/Device/{id}")]
    public class MonitoringController : BaseController
    {
        private readonly IMonitoringService _monitoringService;
        private readonly ILogger<MonitoringController> _logger;

        public MonitoringController(IMonitoringService monitoringService, ILogger<MonitoringController> logger)
        {
            _monitoringService = monitoringService;
            _logger = logger;
        }

        [HttpGet("total-measurementCount")]
        public async Task<ActionResult<int>> GetTotalMeasurementCount(int id)
        {
            try
            {
                var totalMeasurementCount = await _monitoringService.GetTotalMeasurementCount(id);
                return Ok(totalMeasurementCount);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("get-deviceName-measurementsCount")]
        public async Task<ActionResult<IEnumerable<DeviceNameAndNumberOfMeasurements>>> GetDeviceNameAndNumberOfMeasurements(int id)
        {
            try
            {
                var dictionaryDeviceNameCont = await _monitoringService.GetDeviceNameAndNumberOfMeasurements(id);
                return Ok(dictionaryDeviceNameCont);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("min-measurmentValue-atDate")]
        public async Task<ActionResult<IEnumerable<MinMeasurmentValueAtDate>>> CalculateMinValue(int id)
        {
            try
            {
                var minMeasurmentValueAtDate = await _monitoringService.CalculateMinValue(id);
                return Ok(minMeasurmentValueAtDate);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("max-measurmentValue-atDate")]
        public async Task<ActionResult<IEnumerable<MaxMeasurmentValueAtDate>>> CalculateMaxValue( int id)
        {
            try
            {
                var maxMeasurmentValueAtDate = await _monitoringService.CalculateMaxValue(id);
                return Ok(maxMeasurmentValueAtDate);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("average-measurements-value")]
        public async Task<ActionResult<double>> CalculateAverageValue(int id)
        {
            try
            {
                var averageValue = await _monitoringService.CalculateAverageValue(id);
                return Ok(averageValue);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("above-threshold-measurementCount/{threshold}")]
        public async Task<ActionResult<int>> GetMeasurementCountAboveThreshold(int id, double threshold)
        {
            try
            {
                var count = await _monitoringService.GetMeasurementCountAboveThreshold(id, threshold);
                return Ok(count);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("below-threshold-measurementCount/{threshold}")]
        public async Task<ActionResult<int>> GetMeasurementCountBelowThreshold(int id, double threshold)
        {
            try
            {
                var count = await _monitoringService.GetMeasurementCountBelowThreshold(id, threshold);
                return Ok(count);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("averageValue-dateRange/{startDate}/{endDate}")]
        public async Task<ActionResult<double>> CalculateAverageValueForDateRange(int id, DateTime startDate, DateTime endDate)
        {
            try
            {
                var averageValue = await _monitoringService.CalculateAverageValueForDateRange(id, startDate, endDate);
                return Ok(averageValue);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("minValue-dateRange/{startDate}/{endDate}")]
        public async Task<ActionResult<double>> CalculateMinValueForDateRange(int id, DateTime startDate, DateTime endDate)
        {
            try
            {
                var minValue = await _monitoringService.CalculateMinValueForDateRange(id, startDate, endDate);
                return Ok(minValue);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("maxValue-dateRange/{startDate}/{endDate}")]
        public async Task<ActionResult<double>> CalculateMaxValueForDateRange(int id, DateTime startDate, DateTime endDate)
        {
            try
            {
                var maxValue = await _monitoringService.CalculateMaxValueForDateRange(id, startDate, endDate);
                return Ok(maxValue);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("measurements-dateRange/{startDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurementsForDateRange(int id, DateTime startDate, DateTime endDate)
        {
            try
            {
                var measurements = await _monitoringService.GetMeasurementsForDateRange(id, startDate, endDate);
                return Ok(measurements);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("lastest-measurements/{count}")]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetLatestMeasurements(int id, int count)
        {
            try
            {
                var measurements = await _monitoringService.GetLatestMeasurements(id, count);
                return Ok(measurements);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("number-and-averageValue-inHours/{date}")]
        public async Task<ActionResult<IEnumerable< NumberAndAverageValueInHour>>> GetNumberAndAverageValueInHourByDate(int id, DateTime date)
        {
            try
            {
                var numberAndAverageValueInHour = await _monitoringService.GetNumberAndAverageValueInHourByDate(id, date);
                return Ok(numberAndAverageValueInHour);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }
    }
}
