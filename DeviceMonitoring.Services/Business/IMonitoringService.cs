using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects;

namespace DeviceMonitoring.Services.Business;

public interface IMonitoringService
{

    Task<int> GetTotalMeasurementCount(int id);
    Task<int> GetMeasurementCountAboveThreshold(int id, double threshold);
    Task<int> GetMeasurementCountBelowThreshold(int id, double threshold);
    Task<double> CalculateAverageValue(int id);
    Task<double> CalculateMaxValueForDateRange(int id, DateTime startDate, DateTime endDate);
    Task<double> CalculateMinValueForDateRange(int id, DateTime startDate, DateTime endDate);
    Task<double> CalculateAverageValueForDateRange(int id, DateTime startDate, DateTime endDate);
    Task<IEnumerable<MaxMeasurmentValueAtDate>> CalculateMaxValue(int id);
    Task<IEnumerable<MinMeasurmentValueAtDate>> CalculateMinValue(int id);
    Task<IEnumerable<DeviceNameAndNumberOfMeasurements>> GetDeviceNameAndNumberOfMeasurements(int id);
    Task<IEnumerable<Measurement>> GetMeasurementsForDateRange(int id, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Measurement>> GetLatestMeasurements(int id, int count);
    Task<IEnumerable<NumberAndAverageValueInHour>> GetNumberAndAverageValueInHourByDate(int id, DateTime date);
}
