using DeviceMonitoring.Data.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DeviceMonitoring.IntegrationTests.Infrastructure;

public class DeviceMonitoringWebApplicationFactory(
    string connectionString,
    bool useTestAuthentication = true)
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<DeviceMonitoringContext>>();
            services.RemoveAll<DbContextOptions>();
            services.RemoveAll<DeviceMonitoringContext>();

            services.AddDbContext<DeviceMonitoringContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null)));

            if (!useTestAuthentication)
            {
                return;
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.SCHEME_NAME;
                options.DefaultChallengeScheme = TestAuthHandler.SCHEME_NAME;
                options.DefaultForbidScheme = TestAuthHandler.SCHEME_NAME;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.SCHEME_NAME, 
                _ => { });
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await using var scope = Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<DeviceMonitoringContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
    }
}