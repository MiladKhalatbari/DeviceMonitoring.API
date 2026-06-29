using System.Net;
using System.Net.Http.Json;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.IntegrationTests.Infrastructure;
using DeviceMonitoring.Services.DataTransferObjects.Measurements;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DeviceMonitoring.IntegrationTests.Controllers;

[Collection(SqlServerIntegrationTestCollection.NAME)]
public class MeasurementsEndpointsTests : IAsyncLifetime
{
    private readonly DeviceMonitoringWebApplicationFactory _factory;
    private HttpClient _client = null!;

    public MeasurementsEndpointsTests(SqlServerContainerFixture sqlServerContainerFixture)
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
    public async Task GetAllAsync_ShouldReturnSeededMeasurements()
    {
        // Act
        var response = await _client.GetAsync("/api/measurements", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var measurements = await response.Content.ReadFromJsonAsync<List<Measurement>>(TestContext.Current.CancellationToken);

        measurements.Should().NotBeNull();
        measurements.Should().NotBeEmpty();
        measurements.Should().Contain(measurement => measurement.Id == 1);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSeededMeasurement_WhenMeasurementExists()
    {
        // Act
        var response = await _client.GetAsync("/api/measurements/1", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var measurement = await response.Content.ReadFromJsonAsync<Measurement>(TestContext.Current.CancellationToken);

        measurement.Should().NotBeNull();
        measurement!.Id.Should().Be(1);
        measurement.DeviceId.Should().Be(1);
        measurement.Value.Should().Be(10.5);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturn404ProblemDetails_WhenMeasurementDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/measurements/999999", TestContext.Current.CancellationToken);

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
    public async Task CreateAsync_ShouldReturn201AndPersistMeasurement_WhenRequestIsValid()
    {
        // Arrange
        var request = new MeasurementForInsertDto
        {
            DeviceId = 1,
            Value = 18.5
        };

        // Act
        var createResponse = await _client.PostAsJsonAsync("/api/measurements", request, TestContext.Current.CancellationToken);

        // Assert
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdResponse = await createResponse.Content.ReadFromJsonAsync<CreatedIdResponse>(TestContext.Current.CancellationToken);

        createdResponse.Should().NotBeNull();
        createdResponse!.Id.Should().BeGreaterThan(0);

        // Act
        var getResponse = await _client.GetAsync($"/api/measurements/{createdResponse.Id}", TestContext.Current.CancellationToken);

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdMeasurement = await getResponse.Content.ReadFromJsonAsync<Measurement>(TestContext.Current.CancellationToken);

        createdMeasurement.Should().NotBeNull();
        createdMeasurement!.Id.Should().Be(createdResponse.Id);
        createdMeasurement.DeviceId.Should().Be(request.DeviceId);
        createdMeasurement.Value.Should().Be(request.Value);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturn400ValidationProblemDetails_WhenValueIsMissing()
    {
        // Arrange
        var requestWithoutValue = new
        {
            deviceId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/measurements", requestWithoutValue, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Errors.Should().ContainKey(nameof(MeasurementForInsertDto.Value));
        problemDetails.Extensions.Should().ContainKey("traceId");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturn400ValidationProblemDetails_WhenDeviceIdIsZero()
    {
        // Arrange
        var request = new MeasurementForInsertDto
        {
            DeviceId = 0,
            Value = 12.5
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/measurements", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Errors.Should().ContainKey(nameof(MeasurementForInsertDto.DeviceId));
        problemDetails.Extensions.Should().ContainKey("traceId");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturn400ProblemDetails_WhenRouteIdDoesNotMatchBodyId()
    {
        // Arrange
        var request = new MeasurementForUpdateDto
        {
            Id = 2,
            Value = 19.5
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/measurements/1", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Extensions.Should().ContainKey("traceId");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturn204AndPersistUpdatedValue_WhenRequestIsValid()
    {
        // Arrange
        var measurementId = await CreateMeasurementAsync(deviceId: 1, value: 10.5);

        var request = new MeasurementForUpdateDto
        {
            Id = measurementId,
            Value = 25.7
        };

        // Act
        var updateResponse = await _client.PutAsJsonAsync($"/api/measurements/{measurementId}", request, TestContext.Current.CancellationToken);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Act
        var getResponse = await _client.GetAsync($"/api/measurements/{measurementId}", TestContext.Current.CancellationToken);

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedMeasurement = await getResponse.Content.ReadFromJsonAsync<Measurement>(TestContext.Current.CancellationToken);

        updatedMeasurement.Should().NotBeNull();
        updatedMeasurement!.Id.Should().Be(measurementId);
        updatedMeasurement.Value.Should().Be(25.7);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturn204AndMeasurementShouldNotExistAfterward()
    {
        // Arrange
        var measurementId = await CreateMeasurementAsync(deviceId: 1, value: 13.4);

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/measurements/{measurementId}", TestContext.Current.CancellationToken);

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Act
        var getResponse = await _client.GetAsync($"/api/measurements/{measurementId}", TestContext.Current.CancellationToken);

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await getResponse.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status404NotFound);
        problemDetails.Extensions.Should().ContainKey("traceId");
    }

    private async Task<int> CreateMeasurementAsync(int deviceId, double value)
    {
        var request = new MeasurementForInsertDto
        {
            DeviceId = deviceId,
            Value = value
        };

        var response = await _client.PostAsJsonAsync("/api/measurements", request, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdResponse = await response.Content.ReadFromJsonAsync<CreatedIdResponse>(TestContext.Current.CancellationToken);

        createdResponse.Should().NotBeNull();

        return createdResponse!.Id;
    }

    private sealed record CreatedIdResponse(int Id);
}