using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.Business;
using DeviceMonitoring.Services.Configuration;
using DeviceMonitoring.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceMonitoring.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices( this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SECTION_NAME))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IDataIngestionService, DataIngestionService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IMeasurementService, MeasurementService>();
        services.AddScoped<IMonitoringService, MonitoringService>();
        return services;
    }
}