using DeviceMonitoring.Data.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceMonitoring.IntegrationTests.Infrastructure;

public class DeviceMonitoringWebApplicationFactory(string connectionString, bool useTestAuthentication = true): WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DeviceMonitoring"] = connectionString
            });
        });

        if (!useTestAuthentication)
            return;

        builder.ConfigureTestServices(services =>
        {
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
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DeviceMonitoringContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
    }
}