using System.Net;
using System.Net.Http.Json;
using DeviceMonitoring.Data.Context;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.IntegrationTests.Infrastructure;
using DeviceMonitoring.Services.DataTransferObjects.Measurements;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceMonitoring.IntegrationTests.Controllers;

[Collection(SqlServerIntegrationTestCollection.NAME)]
public class DataIngestionEndpointsTests : IAsyncLifetime
{
    private readonly DeviceMonitoringWebApplicationFactory _factory;
    private HttpClient _client = null!;

    public DataIngestionEndpointsTests(SqlServerContainerFixture sqlServerContainerFixture)
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
    public async Task IngestAsync_ShouldCreateDeviceAndMeasurement_WhenDeviceDoesNotExist()
    {
        // Arrange
        var deviceName = $"Integration device {Guid.NewGuid():N}";
        var request = new MeasurementIngestionRequestDto
        {
            DeviceName = deviceName,
            Value = 18.5
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/data/ingest", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var ingestionResponse = await response.Content.ReadFromJsonAsync<MeasurementIngestionResponseDto>(TestContext.Current.CancellationToken);

        ingestionResponse.Should().NotBeNull();
        ingestionResponse!.DeviceId.Should().BeGreaterThan(0);
        ingestionResponse.DeviceCreated.Should().BeTrue();

        // Verify real SQL Server persistence
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DeviceMonitoringContext>();

        var createdDevice = await context.Devices
            .AsNoTracking()
            .SingleAsync(device => device.Id == ingestionResponse.DeviceId, TestContext.Current.CancellationToken);

        var measurements = await context.Measurements
            .AsNoTracking()
            .Where(measurement => measurement.DeviceId == ingestionResponse.DeviceId)
            .ToListAsync(TestContext.Current.CancellationToken);

        createdDevice.Name.Should().Be(deviceName);
        measurements.Should().ContainSingle();
        measurements[0].Value.Should().Be(request.Value!.Value);
    }

    [Fact]
    public async Task IngestAsync_ShouldAddMeasurementWithoutCreatingDevice_WhenDeviceAlreadyExists()
    {
        // Arrange
        const int deviceId = 1;
        const string deviceName = "Device 1";

        var request = new MeasurementIngestionRequestDto
        {
            DeviceName = deviceName,
            Value = 21.7
        };

        var measurementsBefore = await GetMeasurementCountAsync(deviceId);
        var devicesBefore = await GetDeviceCountByNameAsync(deviceName);

        // Act
        var response = await _client.PostAsJsonAsync("/api/data/ingest", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var ingestionResponse = await response.Content.ReadFromJsonAsync<MeasurementIngestionResponseDto>(TestContext.Current.CancellationToken);

        ingestionResponse.Should().NotBeNull();
        ingestionResponse!.DeviceId.Should().Be(deviceId);
        ingestionResponse.DeviceCreated.Should().BeFalse();

        var measurementsAfter = await GetMeasurementCountAsync(deviceId);
        var devicesAfter = await GetDeviceCountByNameAsync(deviceName);

        measurementsAfter.Should().Be(measurementsBefore + 1);
        devicesAfter.Should().Be(devicesBefore);

        var latestMeasurement = await GetLatestMeasurementAsync(deviceId);

        latestMeasurement.Value.Should().Be(request.Value!.Value);
    }

    [Fact]
    public async Task IngestAsync_ShouldReturn400ValidationProblemDetails_WhenDeviceNameIsMissing()
    {
        // Arrange
        var requestWithoutDeviceName = new
        {
            value = 15.5
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/data/ingest", requestWithoutDeviceName, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Errors.Should().ContainKey(nameof(MeasurementIngestionRequestDto.DeviceName));
        problemDetails.Extensions.Should().ContainKey("traceId");
    }

    [Fact]
    public async Task IngestAsync_ShouldReturn400ValidationProblemDetails_WhenDeviceNameIsEmpty()
    {
        // Arrange
        var request = new MeasurementIngestionRequestDto
        {
            DeviceName = string.Empty,
            Value = 15.5
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/data/ingest", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Errors.Should().ContainKey(nameof(MeasurementIngestionRequestDto.DeviceName));
        problemDetails.Extensions.Should().ContainKey("traceId");
    }

    [Fact]
    public async Task IngestAsync_ShouldReturn400ValidationProblemDetails_WhenValueIsMissing()
    {
        // Arrange
        var requestWithoutValue = new
        {
            deviceName = "Device 1"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/data/ingest", requestWithoutValue, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Errors.Should().ContainKey(nameof(MeasurementIngestionRequestDto.Value));
        problemDetails.Extensions.Should().ContainKey("traceId");
    }

    [Fact]
    public async Task IngestAsync_ShouldReturn400ValidationProblemDetails_WhenDeviceNameIsWhitespace()
    {
        // Arrange
        var request = new MeasurementIngestionRequestDto
        {
            DeviceName = "   ",
            Value = 15.5
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/data/ingest",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(
            TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Errors.Should().ContainKey(nameof(MeasurementIngestionRequestDto.DeviceName));
        problemDetails.Extensions.Should().ContainKey("traceId");
    }

    private async Task<int> GetMeasurementCountAsync(int deviceId)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DeviceMonitoringContext>();

        return await context.Measurements.CountAsync(measurement => measurement.DeviceId == deviceId, TestContext.Current.CancellationToken);
    }

    private async Task<int> GetDeviceCountByNameAsync(string deviceName)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DeviceMonitoringContext>();

        return await context.Devices.CountAsync(device => device.Name == deviceName, TestContext.Current.CancellationToken);
    }

    private async Task<Measurement> GetLatestMeasurementAsync(int deviceId)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DeviceMonitoringContext>();

        return await context.Measurements
            .AsNoTracking()
            .Where(measurement => measurement.DeviceId == deviceId)
            .OrderByDescending(measurement => measurement.Id)
            .FirstAsync(TestContext.Current.CancellationToken);
    }
}