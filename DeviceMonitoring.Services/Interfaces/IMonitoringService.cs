using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.MonitoringModels;

namespace DeviceMonitoring.Services.Interfaces;

public interface IMonitoringService
{
    Task<double?> CalculateAverageValue(int id);
    Task<double?> CalculateAverageValueForDateRange(int id, DateTime startDate, DateTime endDate);
    Task<double?> CalculateMinValueForDateRange(int id, DateTime startDate, DateTime endDate);
    Task<double?> CalculateMaxValueForDateRange(int id, DateTime startDate, DateTime endDate);
    Task<DeviceNameAndNumberOfMeasurements> GetDeviceNameAndNumberOfMeasurements(int id);

    Task<int> GetTotalMeasurementCount(int id);
    Task<int> GetMeasurementCountAboveThreshold(int id, double threshold);
    Task<int> GetMeasurementCountBelowThreshold(int id, double threshold);

    Task<IEnumerable<MaxMeasurmentValueAtDate>> CalculateMaxValue(int id);
    Task<IEnumerable<MinMeasurmentValueAtDate>> CalculateMinValue(int id);
    Task<IEnumerable<Measurement>> GetMeasurementsForDateRange(int id, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Measurement>> GetLatestMeasurements(int id, int count);
    Task<IEnumerable<NumberAndAverageValueInHour>> GetNumberAndAverageValueInHourByDate(int id, DateTime date);
}
