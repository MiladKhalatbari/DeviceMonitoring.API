using DeviceMonitoring.Data.Context;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviceMonitoring.Services.Business;

public class MonitoringService : IMonitoringService
{
    private readonly DeviceMonitoringContext _context;
    public MonitoringService(DeviceMonitoringContext context)
    {
        _context = context;
    }
    public async Task<double> CalculateAverageValue(int id)
    {
        double averageValue = await _context.Measurements
                .Where(m => m.DeviceId == id)
                .AverageAsync(m => m.Value);

        return averageValue;
    }

    public async Task<double> CalculateAverageValueForDateRange(int id, DateTime startDate, DateTime endDate)
    {
        double averageValue = await _context.Measurements.
            Where(m => m.DeviceId == id && m.CreatedOn >= startDate && m.CreatedOn <= endDate).
            AverageAsync(m => m.Value);
        return averageValue;
    }

    public async Task<IEnumerable<MaxMeasurmentValueAtDate>> CalculateMaxValue(int id)
    {
        double maxValue = await _context.Measurements.Where(m => m.DeviceId == id).MaxAsync(m => m.Value);
        return await _context.Measurements.
            Where(m => m.DeviceId == id && m.Value == maxValue).
            Select(x => new MaxMeasurmentValueAtDate { AtDate = x.CreatedOn, MaxValue = x.Value }).
            ToListAsync();
    }

    public async Task<double> CalculateMaxValueForDateRange(int id, DateTime startDate, DateTime endDate)
    {
        double maxValue = await _context.Measurements.Where(m => m.DeviceId == id && m.CreatedOn >= startDate && m.CreatedOn <= endDate).MaxAsync(m => m.Value);
        return maxValue;
    }

    public async Task<IEnumerable<MinMeasurmentValueAtDate>> CalculateMinValue(int id)
    {
        double minValue = await _context.Measurements.Where(m => m.DeviceId == id).MinAsync(m => m.Value);
        return await _context.Measurements.
            Where(m => m.DeviceId == id && m.Value == minValue).
            Select(x => new MinMeasurmentValueAtDate { AtDate = x.CreatedOn, MinValue = x.Value }).
            ToListAsync();
    }

    public async Task<double> CalculateMinValueForDateRange(int id, DateTime startDate, DateTime endDate)
    {
        double minValue = await _context.Measurements.Where(m => m.DeviceId == id && m.CreatedOn >= startDate && m.CreatedOn <= endDate).MinAsync(m => m.Value);
        return minValue;
    }
    public async Task<IEnumerable<DeviceNameAndNumberOfMeasurements>> GetDeviceNameAndNumberOfMeasurements(int id = 0)
    {

        if (id != 0)
        {
            return await _context.Devices.
                Where(m => m.Id == id).
                Include(d => d.Measurements).
                GroupBy(x => x.Name).
                Select((x) => new DeviceNameAndNumberOfMeasurements { DeviceName = x.Key, NumberOfMeasurements = x.SelectMany(x => x.Measurements).Count() }).
                ToListAsync();
        }

        return await _context.Devices.
            Include(d => d.Measurements).
            GroupBy(x => x.Name).
            Select((x) => new DeviceNameAndNumberOfMeasurements { DeviceName = x.Key, NumberOfMeasurements = x.SelectMany(x => x.Measurements).Count() }).
            ToListAsync();
    }
    public async Task<IEnumerable<NumberAndAverageValueInHour>> GetNumberAndAverageValueInHourByDate(int id, DateTime date)
    {

        var numberAndAverageValueInHour = await _context.Measurements
              .Where(m => m.DeviceId == id && m.CreatedOn.Date == date.Date).GroupBy(m => m.CreatedOn.Hour)
              .Select(group => new NumberAndAverageValueInHour
              {
                  AtHour = group.Key,
                  NumberOfMeasurments = group.Count(),
                  AverageValue = group.Average(m => m.Value)
              }).ToListAsync();
             
        return numberAndAverageValueInHour;
    }
    public async Task<IEnumerable<Measurement>> GetLatestMeasurements(int id, int count)
    {
        return await _context.Measurements.Where(m => m.DeviceId == id).OrderBy(x => x.CreatedOn).TakeLast(count).ToListAsync();
    }

    public async Task<int> GetMeasurementCountAboveThreshold(int id, double threshold)
    {
        return await _context.Measurements.Where(m => m.DeviceId == id && m.Value > threshold).CountAsync();
    }

    public async Task<int> GetMeasurementCountBelowThreshold(int id, double threshold)
    {
        return await _context.Measurements.Where(m => m.DeviceId == id && m.Value < threshold).CountAsync();
    }

    public async Task<IEnumerable<Measurement>> GetMeasurementsForDateRange(int id, DateTime startDate, DateTime endDate)
    {
        var result = await _context.Measurements.Where(m => m.DeviceId == id).ToListAsync();
        result = result.Where(m => m.CreatedOn <= endDate && m.CreatedOn >= startDate).ToList();
        return result;
    }

    public async Task<int> GetTotalMeasurementCount(int id)
    {
        return await _context.Measurements.Where(m => m.DeviceId == id).CountAsync();
    }
}
