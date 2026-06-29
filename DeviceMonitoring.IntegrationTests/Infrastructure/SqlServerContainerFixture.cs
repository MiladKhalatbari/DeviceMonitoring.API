using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
using Xunit.Internal;

namespace DeviceMonitoring.IntegrationTests.Infrastructure;

public class SqlServerContainerFixture : IAsyncLifetime
{
    private const string DATABASE_NAME = "DeviceMonitoringIntegrationTests";

    private readonly MsSqlContainer _sqlServerContainer =
        new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
            .Build();

    public string ConnectionString
    {
        get
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(
                _sqlServerContainer.GetConnectionString())
            {
                InitialCatalog = DATABASE_NAME
            };

            return connectionStringBuilder.ConnectionString;
        }
    }

    public async ValueTask InitializeAsync()
    {
        var task = _sqlServerContainer.StartAsync();
        if (task != null)
        {
            await task;
        }
    }

    public ValueTask DisposeAsync()
    {
        return _sqlServerContainer.DisposeAsync();
    }
}