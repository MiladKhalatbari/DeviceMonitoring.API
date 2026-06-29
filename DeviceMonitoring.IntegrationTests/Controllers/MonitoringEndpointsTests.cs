using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.IntegrationTests.Infrastructure;
using DeviceMonitoring.Services.DataTransferObjects.MonitoringModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DeviceMonitoring.IntegrationTests.Controllers;

[Collection(SqlServerIntegrationTestCollection.NAME)]
public class MonitoringEndpointsTests : IAsyncLifetime
{
    private readonly DeviceMonitoringWebApplicationFactory _factory;
    private HttpClient _client = null!;

    public MonitoringEndpointsTests(SqlServerContainerFixture sqlServerContainerFixture)
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
    public async Task GetTotalMeasurementCountAsync_ShouldReturnSeededCount()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/total-measurement-count",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var count = await response.Content.ReadFromJsonAsync<int>(
            TestContext.Current.CancellationToken);

        count.Should().Be(5);
    }

    [Fact]
    public async Task GetDeviceSummaryAsync_ShouldReturnSeededDeviceSummary()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/summary",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var summary = await response.Content.ReadFromJsonAsync<DeviceNameAndNumberOfMeasurements>(
            TestContext.Current.CancellationToken);

        summary.Should().NotBeNull();
        summary!.DeviceName.Should().Be("Device 1");
        summary.NumberOfMeasurements.Should().Be(5);
    }

    [Fact]
    public async Task GetMinimumValuesByDateAsync_ShouldReturnResultsForEachSeededDate()
    {
        // Act
        var response = await _client.GetAsync("/api/monitoring/devices/1/minimum-values-by-date", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var document = await ReadJsonDocumentAsync(response);

        document.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
        document.RootElement.GetArrayLength().Should().Be(2);
    }

    [Fact]
    public async Task GetMaximumValuesByDateAsync_ShouldReturnResultsForEachSeededDate()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/maximum-values-by-date",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var document = await ReadJsonDocumentAsync(response);

        document.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
        document.RootElement.GetArrayLength().Should().Be(2);
    }

    [Fact]
    public async Task GetAverageValueAsync_ShouldReturnCorrectAverage()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/average-value",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var averageValue = await response.Content.ReadFromJsonAsync<double?>(
            TestContext.Current.CancellationToken);

        averageValue.Should().NotBeNull();
        averageValue!.Value.Should().BeApproximately(12.48, 0.001);
    }

    [Fact]
    public async Task GetMeasurementCountAboveThresholdAsync_ShouldReturnCorrectCount()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/measurement-count/above-threshold?threshold=12",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var count = await response.Content.ReadFromJsonAsync<int>(
            TestContext.Current.CancellationToken);

        count.Should().Be(3);
    }

    [Fact]
    public async Task GetMeasurementCountBelowThresholdAsync_ShouldReturnCorrectCount()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/measurement-count/below-threshold?threshold=12",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var count = await response.Content.ReadFromJsonAsync<int>(
            TestContext.Current.CancellationToken);

        count.Should().Be(2);
    }

    [Fact]
    public async Task GetAverageValueForDateRangeAsync_ShouldReturnCorrectAverage()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/average-value/date-range?startDate=2023-08-20&endDate=2023-08-20",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var averageValue = await response.Content.ReadFromJsonAsync<double?>(
            TestContext.Current.CancellationToken);

        averageValue.Should().NotBeNull();
        averageValue!.Value.Should().BeApproximately(11.875, 0.001);
    }

    [Fact]
    public async Task GetMinimumValueForDateRangeAsync_ShouldReturnCorrectMinimum()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/minimum-value/date-range?startDate=2023-08-20&endDate=2023-08-20",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var minimumValue = await response.Content.ReadFromJsonAsync<double?>(
            TestContext.Current.CancellationToken);

        minimumValue.Should().NotBeNull();
        minimumValue!.Value.Should().BeApproximately(9.2, 0.001);
    }

    [Fact]
    public async Task GetMaximumValueForDateRangeAsync_ShouldReturnCorrectMaximum()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/maximum-value/date-range?startDate=2023-08-20&endDate=2023-08-20",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var maximumValue = await response.Content.ReadFromJsonAsync<double?>(
            TestContext.Current.CancellationToken);

        maximumValue.Should().NotBeNull();
        maximumValue!.Value.Should().BeApproximately(14.9, 0.001);
    }

    [Fact]
    public async Task GetMeasurementsForDateRangeAsync_ShouldReturnMeasurementsFromRequestedDate()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/measurements/date-range?startDate=2023-08-20&endDate=2023-08-20",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var measurements = await response.Content.ReadFromJsonAsync<List<Measurement>>(
            TestContext.Current.CancellationToken);

        measurements.Should().NotBeNull();
        measurements.Should().HaveCount(4);
        measurements.Should().OnlyContain(measurement => measurement.DeviceId == 1);
        measurements.Should().OnlyContain(measurement => measurement.CreatedOn.Date == new DateTime(2023, 8, 20));
    }

    [Fact]
    public async Task GetLatestMeasurementsAsync_ShouldReturnLatestMeasurements()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/latest-measurements?count=2",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var measurements = await response.Content.ReadFromJsonAsync<List<Measurement>>(
            TestContext.Current.CancellationToken);

        measurements.Should().NotBeNull();
        measurements.Should().HaveCount(2);

        measurements![0].CreatedOn.Should().BeOnOrAfter(measurements[1].CreatedOn);
        measurements[0].Value.Should().BeApproximately(14.9, 0.001);
        measurements[1].Value.Should().BeApproximately(12.9, 0.001);
    }

    [Fact]
    public async Task GetHourlySummaryAsync_ShouldReturnHourlyGroupsForSeededDate()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/hourly-summary?date=2023-08-20",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var document = await ReadJsonDocumentAsync(response);

        document.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
        document.RootElement.GetArrayLength().Should().Be(3);
    }

    [Fact]
    public async Task GetTotalMeasurementCountAsync_ShouldReturn404ProblemDetails_WhenDeviceDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/999/total-measurement-count",
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(response, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetLatestMeasurementsAsync_ShouldReturn400ProblemDetails_WhenCountIsZero()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/latest-measurements?count=0",
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(response, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAverageValueForDateRangeAsync_ShouldReturn400ProblemDetails_WhenStartDateIsAfterEndDate()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/monitoring/devices/1/average-value/date-range?startDate=2023-08-21&endDate=2023-08-20",
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(response, HttpStatusCode.BadRequest);
    }

    private static async Task<JsonDocument> ReadJsonDocumentAsync(HttpResponseMessage response)
    {
        var document = await response.Content.ReadFromJsonAsync<JsonDocument>(
            TestContext.Current.CancellationToken);

        document.Should().NotBeNull();

        return document!;
    }

    private static async Task AssertProblemDetailsAsync(HttpResponseMessage response, HttpStatusCode expectedStatusCode)
    {
        response.StatusCode.Should().Be(expectedStatusCode);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(
            TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be((int)expectedStatusCode);
        problemDetails.Extensions.Should().ContainKey("traceId");
    }
}