using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.IntegrationTests.Infrastructure;
using DeviceMonitoring.Services.DataTransferObjects.Devices;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace DeviceMonitoring.IntegrationTests.Controllers;

[Collection(SqlServerIntegrationTestCollection.NAME)]
public class DevicesEndpointsTests : IAsyncLifetime
{
    private readonly DeviceMonitoringWebApplicationFactory _factory;
    private HttpClient _client = null!;

    public DevicesEndpointsTests(SqlServerContainerFixture sqlServerContainerFixture)
    {
        _factory = new DeviceMonitoringWebApplicationFactory(sqlServerContainerFixture.ConnectionString);
    }
    public async ValueTask InitializeAsync()
    {
        await _factory.ResetDatabaseAsync();

        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    public ValueTask DisposeAsync()
    {
        _client.Dispose();
        _factory.Dispose();

        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturn404ProblemDetails_WhenDeviceDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/devices/999", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status404NotFound);
        problemDetails.Title.Should().Be("Resource not found");
        problemDetails.Extensions.Should().ContainKey("traceId");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSeededDevice_WhenDeviceExists()
    {
        // Act
        var response = await _client.GetAsync("/api/devices/1", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var device = await response.Content.ReadFromJsonAsync<Device>(TestContext.Current.CancellationToken);

        device.Should().NotBeNull();
        device!.Id.Should().Be(1);
        device.Name.Should().Be("Device 1");
        device.Location.Should().Be("Location A");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturn201AndCreatedDevice_WhenRequestIsValid()
    {
        // Arrange
        var request = CreateValidDeviceRequest($"Integration device {Guid.NewGuid():N}");

        // Act
        var createResponse = await _client.PostAsJsonAsync("/api/devices", request, TestContext.Current.CancellationToken);

        // Assert
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdResponse = await createResponse.Content.ReadFromJsonAsync<CreatedIdResponse>(TestContext.Current.CancellationToken);

        createdResponse.Should().NotBeNull();
        createdResponse!.Id.Should().BeGreaterThan(0);

        // Act
        var getResponse = await _client.GetAsync($"/api/devices/{createdResponse.Id}", TestContext.Current.CancellationToken);

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdDevice = await getResponse.Content.ReadFromJsonAsync<Device>(TestContext.Current.CancellationToken);

        createdDevice.Should().NotBeNull();
        createdDevice!.Id.Should().Be(createdResponse.Id);
        createdDevice.Name.Should().Be(request.Name);
        createdDevice.Location.Should().Be(request.Location);
        createdDevice.Manufacturer.Should().Be(request.Manufacturer);
        createdDevice.ModelNumber.Should().Be(request.ModelNumber);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturn400ValidationProblemDetails_WhenNameIsEmpty()
    {
        // Arrange
        var request = new DeviceForInsertDto
        {
            Name = string.Empty,
            Location = "Prague",
            Manufacturer = "Test manufacturer",
            ModelNumber = "Test model"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/devices", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be((int)HttpStatusCode.BadRequest);
        problemDetails.Errors.Should().ContainKey(nameof(DeviceForInsertDto.Name));
        problemDetails.Extensions.Should().ContainKey("traceId");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturn400ProblemDetails_WhenRouteIdDoesNotMatchBodyId()
    {
        // Arrange
        var request = new DeviceForUpdateDto
        {
            Id = 2,
            Location = "Updated location",
            IsActive = true
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/devices/1", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be((int)HttpStatusCode.BadRequest);
        problemDetails.Extensions.Should().ContainKey("traceId");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturn204AndDeviceShouldNotExistAfterward()
    {
        // Arrange
        var deviceId = await CreateDeviceAsync($"Device to delete {Guid.NewGuid():N}");

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/devices/{deviceId}", TestContext.Current.CancellationToken);

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Act
        var getResponse = await _client.GetAsync($"/api/devices/{deviceId}", TestContext.Current.CancellationToken);

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await getResponse.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be((int)HttpStatusCode.NotFound);
        problemDetails.Extensions.Should().ContainKey("traceId");
    }

    private async Task<int> CreateDeviceAsync(string name)
    {
        var request = CreateValidDeviceRequest(name);

        var response = await _client.PostAsJsonAsync("/api/devices", request, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdResponse = await response.Content.ReadFromJsonAsync<CreatedIdResponse>(TestContext.Current.CancellationToken);

        createdResponse.Should().NotBeNull();

        return createdResponse!.Id;
    }

    private static DeviceForInsertDto CreateValidDeviceRequest(string name)
    {
        return new DeviceForInsertDto
        {
            Name = name,
            Location = "Prague",
            Manufacturer = "Integration Test Manufacturer",
            ModelNumber = "Integration-Test-001"
        };
    }
    private record CreatedIdResponse(int Id);
}