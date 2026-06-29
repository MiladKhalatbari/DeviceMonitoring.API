using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.Measurements;

namespace DeviceMonitoring.Services.Interfaces;

public interface IMeasurementService
{
    Task<IEnumerable<Measurement>> GetAllAsync();

    Task<Measurement> GetByIdAsync(int id);

    Task<int> AddAsync(MeasurementForInsertDto measurement);

    Task<int> UpdateAsync(MeasurementForUpdateDto measurement);

    Task<int> DeleteAsync(int id);
}