using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.Business;
using DeviceMonitoring.Services.DataTransferObjects.Devices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.API.Controllers
{

    public class DevicesController : BaseController
    {
        private readonly IDeviceService _deviceService;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(IDeviceService deviceService, ILogger<DevicesController> logger)
        {
            _deviceService = deviceService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> Get()
        {
            try
            {
                var devices = await _deviceService.GetAllAsync();
                return Ok(devices);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<Device>> GetById(int id)
        {
            try
            {
                var device = await _deviceService.GetByIdAsync(id);
                return Ok(device);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(DeviceForInsertDto deviceForInsert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _deviceService.AddAsync(deviceForInsert);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> Update(int id, DeviceForUpdateDto deviceForUpdate)
        {
            if (!ModelState.IsValid || id != deviceForUpdate.Id)
            {
                return BadRequest();
            }

            try
            {
                await _deviceService.UpdateAsync(deviceForUpdate);
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
                await _deviceService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now.ToShortTimeString}_{e.Message}");
                return BadRequest();
            }
        }
    }
}