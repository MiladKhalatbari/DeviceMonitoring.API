namespace DeviceMonitoring.IntegrationTests.Infrastructure;

[CollectionDefinition(NAME)]
public class SqlServerIntegrationTestCollection : ICollectionFixture<SqlServerContainerFixture>
{
    public const string NAME = "SQL Server integration tests";
}