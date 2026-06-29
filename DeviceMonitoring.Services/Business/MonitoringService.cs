using DeviceMonitoring.Data.Context;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.MonitoringModels;
using DeviceMonitoring.Services.Exceptions;
using DeviceMonitoring.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeviceMonitoring.Services.Business;

public sealed class MonitoringService : IMonitoringService
{
    private readonly DeviceMonitoringContext _context;

    public MonitoringService(DeviceMonitoringContext context) => _context = context;

    public async Task<double?> CalculateAverageValue(int id)
    {
        await EnsureDeviceExistsAsync(id);

        return await GetMeasurementsQuery(id)
            .Select(measurement => (double?)measurement.Value)
            .AverageAsync();
    }

    public async Task<double?> CalculateAverageValueForDateRange(int id, DateTime startDate, DateTime endDate)
    {
        await EnsureDeviceExistsAsync(id);
        var (start, endExclusive) = GetWholeDayRange(startDate, endDate);

        return await GetMeasurementsQuery(id)
            .Where(measurement => measurement.CreatedOn >= start && measurement.CreatedOn < endExclusive)
            .Select(measurement => (double?)measurement.Value)
            .AverageAsync();
    }

    public async Task<IEnumerable<MaxMeasurmentValueAtDate>> CalculateMaxValue(int id)
    {
        await EnsureDeviceExistsAsync(id);

        return await GetMeasurementsQuery(id)
            .GroupBy(measurement => measurement.CreatedOn.Date)
            .OrderBy(group => group.Key)
            .Select(group => new MaxMeasurmentValueAtDate
            {
                AtDate = group.Key,
                MaxValue = group.Max(measurement => measurement.Value)
            })
            .ToListAsync();
    }

    public async Task<double?> CalculateMaxValueForDateRange(int id, DateTime startDate, DateTime endDate)
    {
        await EnsureDeviceExistsAsync(id);
        var (start, endExclusive) = GetWholeDayRange(startDate, endDate);

        return await GetMeasurementsQuery(id)
            .Where(measurement => measurement.CreatedOn >= start && measurement.CreatedOn < endExclusive)
            .Select(measurement => (double?)measurement.Value)
            .MaxAsync();
    }

    public async Task<IEnumerable<MinMeasurmentValueAtDate>> CalculateMinValue(int id)
    {
        await EnsureDeviceExistsAsync(id);

        return await GetMeasurementsQuery(id)
            .GroupBy(measurement => measurement.CreatedOn.Date)
            .OrderBy(group => group.Key)
            .Select(group => new MinMeasurmentValueAtDate
            {
                AtDate = group.Key,
                MinValue = group.Min(measurement => measurement.Value)
            })
            .ToListAsync();
    }

    public async Task<double?> CalculateMinValueForDateRange(int id, DateTime startDate, DateTime endDate)
    {
        await EnsureDeviceExistsAsync(id);
        var (start, endExclusive) = GetWholeDayRange(startDate, endDate);

        return await GetMeasurementsQuery(id)
            .Where(measurement => measurement.CreatedOn >= start && measurement.CreatedOn < endExclusive)
            .Select(measurement => (double?)measurement.Value)
            .MinAsync();
    }

    public async Task<DeviceNameAndNumberOfMeasurements> GetDeviceNameAndNumberOfMeasurements(int id)
    {
        ValidateDeviceId(id);

        var summary = await _context.Devices
            .AsNoTracking()
            .Where(device => device.Id == id)
            .Select(device => new DeviceNameAndNumberOfMeasurements
            {
                DeviceName = device.Name,
                NumberOfMeasurements = device.Measurements.Count()
            })
            .SingleOrDefaultAsync();

        return summary ?? throw new ResourceNotFoundException("Device", id);
    }

    public async Task<IEnumerable<NumberAndAverageValueInHour>> GetNumberAndAverageValueInHourByDate(int id, DateTime date)
    {
        await EnsureDeviceExistsAsync(id);

        var start = date.Date;
        var endExclusive = start.AddDays(1);

        return await GetMeasurementsQuery(id)
            .Where(measurement => measurement.CreatedOn >= start && measurement.CreatedOn < endExclusive)
            .GroupBy(measurement => measurement.CreatedOn.Hour)
            .OrderBy(group => group.Key)
            .Select(group => new NumberAndAverageValueInHour
            {
                AtHour = group.Key,
                NumberOfMeasurements = group.Count(),
                AverageValue = group.Average(measurement => measurement.Value)
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<Measurement>> GetLatestMeasurements(int id, int count)
    {
        ValidateCount(count);
        await EnsureDeviceExistsAsync(id);

        return await GetMeasurementsQuery(id)
            .OrderByDescending(measurement => measurement.CreatedOn)
            .ThenByDescending(measurement => measurement.Id)
            .Take(count)
            .ToListAsync();
    }

    public async Task<int> GetMeasurementCountAboveThreshold(int id, double threshold)
    {
        ValidateThreshold(threshold);
        await EnsureDeviceExistsAsync(id);

        return await GetMeasurementsQuery(id)
            .CountAsync(measurement => measurement.Value > threshold);
    }

    public async Task<int> GetMeasurementCountBelowThreshold(int id, double threshold)
    {
        ValidateThreshold(threshold);
        await EnsureDeviceExistsAsync(id);

        return await GetMeasurementsQuery(id)
            .CountAsync(measurement => measurement.Value < threshold);
    }

    public async Task<IEnumerable<Measurement>> GetMeasurementsForDateRange(int id, DateTime startDate, DateTime endDate)
    {
        await EnsureDeviceExistsAsync(id);
        var (start, endExclusive) = GetWholeDayRange(startDate, endDate);

        return await GetMeasurementsQuery(id)
            .Where(measurement => measurement.CreatedOn >= start && measurement.CreatedOn < endExclusive)
            .OrderBy(measurement => measurement.CreatedOn)
            .ToListAsync();
    }

    public async Task<int> GetTotalMeasurementCount(int id)
    {
        await EnsureDeviceExistsAsync(id);

        return await GetMeasurementsQuery(id).CountAsync();
    }

    private IQueryable<Measurement> GetMeasurementsQuery(int deviceId)
    {
        return _context.Measurements
            .AsNoTracking()
            .Where(measurement => measurement.DeviceId == deviceId);
    }

    private async Task EnsureDeviceExistsAsync(int id)
    {
        ValidateDeviceId(id);

        var exists = await _context.Devices
            .AsNoTracking()
            .AnyAsync(device => device.Id == id);

        if (!exists)
            throw new ResourceNotFoundException("Device", id);
    }

    private static void ValidateDeviceId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Device id must be greater than zero.", nameof(id));
    }

    private static (DateTime Start, DateTime EndExclusive) GetWholeDayRange(DateTime startDate, DateTime endDate)
    {
        if (startDate.Date > endDate.Date)
            throw new ArgumentException("Start date cannot be after end date.");

        return (startDate.Date, endDate.Date.AddDays(1));
    }

    private static void ValidateCount(int count)
    {
        if (count <= 0)
            throw new ArgumentException("Count must be greater than zero.", nameof(count));
    }

    private static void ValidateThreshold(double threshold)
    {
        if (!double.IsFinite(threshold))
            throw new ArgumentException("Threshold must be a valid finite number.", nameof(threshold));
    }
}