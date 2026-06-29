using DeviceMonitoring.Data.Repositories;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.Measurements;
using DeviceMonitoring.Services.Exceptions;
using DeviceMonitoring.Services.Interfaces;

namespace DeviceMonitoring.Services.Business;

public sealed class MeasurementService(IRepository<Measurement> measurementRepository) : IMeasurementService
{
    private readonly IRepository<Measurement> _measurementRepository = measurementRepository;

    public async Task<IEnumerable<Measurement>> GetAllAsync()
    {
        return await _measurementRepository.GetAllAsync();
    }

    public async Task<Measurement> GetByIdAsync(int id)
    {
        var entity = await _measurementRepository.GetAsync(id);

        if (entity is null)
        {
            throw new ResourceNotFoundException("Measurement", id);
        }

        return entity;
    }

    public async Task<int> AddAsync(MeasurementForInsertDto measurement)
    {
        ArgumentNullException.ThrowIfNull(measurement);

        var entity = new Measurement
        {
            Value = measurement.Value,
            DeviceId = measurement.DeviceId,
            CreatedOn = DateTime.UtcNow
        };

        await _measurementRepository.AddAsync(entity);

        return entity.Id;
    }

    public async Task<int> UpdateAsync(MeasurementForUpdateDto measurement)
    {
        ArgumentNullException.ThrowIfNull(measurement);

        var entity = await GetByIdAsync(measurement.Id);

        entity.Value = measurement.Value;

        await _measurementRepository.UpdateAsync(entity);

        return entity.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);

        await _measurementRepository.DeleteAsync(entity);

        return entity.Id;
    }
}