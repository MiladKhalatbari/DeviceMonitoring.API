using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

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
        await _sqlServerContainer.StartAsync();
        var adminConnectionString = _sqlServerContainer.GetConnectionString();
        var builder = new SqlConnectionStringBuilder(adminConnectionString);

        Console.WriteLine($"Test SQL Server endpoint: {builder.DataSource}");
        await using var connection = new SqlConnection(adminConnectionString);
        await connection.OpenAsync();
    }

    public ValueTask DisposeAsync()
    {
        return _sqlServerContainer.DisposeAsync();
    }
}