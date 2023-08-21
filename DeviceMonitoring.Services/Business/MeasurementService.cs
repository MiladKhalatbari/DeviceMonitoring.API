using DeviceMonitoring.Data.Repositories;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.Measurments;

namespace DeviceMonitoring.Services.Business;

public class MeasurementService : IMeasurementService
{
    private readonly IRepository<Measurement> _measurementRepository;

    public MeasurementService(IRepository<Measurement> measurementRepository)
    {
        _measurementRepository = measurementRepository;
    }

    public async Task<IEnumerable<Measurement>> GetAllAsync()
    {
        return await _measurementRepository.GetAllAsync();
    }

    public async Task<Measurement> GetByIdAsync(int id)
    {
        var entity = await _measurementRepository.GetAsync(id);
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return entity;
    }

    public async Task<int> AddAsync(MeasurementForInsertDto measurement)
    {
        var entity = new Measurement
        {
            Value = measurement.Value,
            DeviceId = measurement.DeviceId,
            CreatedOn = DateTime.Now
        };

        await _measurementRepository.AddAsync(entity);
        return entity.Id;
    }

    public async Task<int> UpdateAsync(MeasurementForUpdateDto measurement)
    {
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