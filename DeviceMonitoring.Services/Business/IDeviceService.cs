using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.Devices;

namespace DeviceMonitoring.Services.Business;

public interface IDeviceService
{
    Task<IEnumerable<Device>> GetAllAsync();

    Task<Device> GetByIdAsync(int id);

    Task<Device> GetByNameAsync(string name);

    Task<int> AddAsync(DeviceForInsertDto device);

    Task<int> UpdateAsync(DeviceForUpdateDto device);

    Task<int> DeleteAsync(int id);
}