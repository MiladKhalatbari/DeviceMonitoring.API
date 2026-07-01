using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace DeviceMonitoring.IntegrationTests.Infrastructure;

public sealed class SqlServerContainerFixture : IAsyncLifetime
{
    private const string DATABASE_NAME = "DeviceMonitoringIntegrationTests";
    private const string CI_CONNECTION_STRING_VARIABLE = "INTEGRATION_TEST_CONNECTION_STRING";

    private readonly string? _ciConnectionString =
        Environment.GetEnvironmentVariable(CI_CONNECTION_STRING_VARIABLE);

    private readonly MsSqlContainer? _sqlServerContainer;

    public SqlServerContainerFixture()
    {
        if (string.IsNullOrWhiteSpace(_ciConnectionString))
        {
            _sqlServerContainer =
                new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
                    .Build();
        }
    }

    public string ConnectionString =>
        _ciConnectionString ?? GetTestcontainersConnectionString();

    public async ValueTask InitializeAsync()
    {
        if (_sqlServerContainer is not null)
        {
            await _sqlServerContainer.StartAsync();
        }

        var masterConnectionString = new SqlConnectionStringBuilder(ConnectionString)
        {
            InitialCatalog = "master"
        }.ConnectionString;

        await using var connection = new SqlConnection(masterConnectionString);
        await connection.OpenAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_sqlServerContainer is not null)
        {
            await _sqlServerContainer.DisposeAsync();
        }
    }

    private string GetTestcontainersConnectionString()
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(
            _sqlServerContainer!.GetConnectionString())
        {
            InitialCatalog = DATABASE_NAME
        };

        return connectionStringBuilder.ConnectionString;
    }
}