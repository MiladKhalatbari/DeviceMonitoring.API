using DeviceMonitoring.Data.Context;
using DeviceMonitoring.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceMonitoring.Data;

public static class DataServiceRegistration
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddDbContext<DeviceMonitoringContext>(options =>
        options.UseSqlServer(
            connectionString,
            sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null)));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}