using DeviceMonitoring.Data.Repositories;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.Devices;

namespace DeviceMonitoring.Services.Business;

public class DeviceService : IDeviceService
{
    private readonly IRepository<Device> _deviceRepository;

    public DeviceService(IRepository<Device> deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<IEnumerable<Device>> GetAllAsync()
    {
        return await _deviceRepository.GetAllAsync();
    }

    public async Task<Device> GetByIdAsync(int id)
    {
        var entity = await _deviceRepository.GetAsync(id);
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return entity;
    }

    public async Task<Device> GetByNameAsync(string name)
    {
        var entity = await _deviceRepository.GetAllAsync(filters: p => p.Name.Equals(name));
        return entity.FirstOrDefault();
    }

    public async Task<int> AddAsync(DeviceForInsertDto device)
    {
        var entity = new Device
        {
            Name = device.Name,
            ModelNumber = device.ModelNumber,
            Manufacturer = device.Manufacturer,
            Location = device.Location,
            InstallationDate = DateTime.Now,
            IsActive = true,
        };

        await _deviceRepository.AddAsync(entity);
        return entity.Id;
    }

    public async Task<int> UpdateAsync(DeviceForUpdateDto device)
    {
        var entity = await GetByIdAsync(device.Id);

        entity.IsActive = device.IsActive;
        entity.Location = device.Location;

        await _deviceRepository.UpdateAsync(entity);
        return entity.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        await _deviceRepository.DeleteAsync(entity);
        return entity.Id;

    }
}