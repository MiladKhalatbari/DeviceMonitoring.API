using DeviceMonitoring.API.Controllers;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.MonitoringModels;
using DeviceMonitoring.Services.Exceptions;
using DeviceMonitoring.Services.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.Tests;

public sealed class MonitoringControllerTests
{
    private readonly IMonitoringService _monitoringService;
    private readonly MonitoringController _controller;

    public MonitoringControllerTests()
    {
        _monitoringService = A.Fake<IMonitoringService>();
        _controller = new MonitoringController(_monitoringService);
    }

    [Fact]
    public async Task GetTotalMeasurementCountAsync_ShouldReturnOkWithCount()
    {
        const int deviceId = 1;
        const int expectedCount = 4;

        A.CallTo(() => _monitoringService.GetTotalMeasurementCount(deviceId))
            .Returns(Task.FromResult(expectedCount));

        var result = await _controller.GetTotalMeasurementCountAsync(deviceId);

        AssertOk(result, expectedCount);

        A.CallTo(() => _monitoringService.GetTotalMeasurementCount(deviceId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetDeviceSummaryAsync_ShouldReturnOkWithSummary()
    {
        const int deviceId = 1;
        var expected = new DeviceNameAndNumberOfMeasurements
        {
            DeviceName = "Device 1",
            NumberOfMeasurements = 3
        };

        A.CallTo(() => _monitoringService.GetDeviceNameAndNumberOfMeasurements(deviceId))
            .Returns(Task.FromResult(expected));

        var result = await _controller.GetDeviceSummaryAsync(deviceId);

        AssertOk(result, expected);

        A.CallTo(() => _monitoringService.GetDeviceNameAndNumberOfMeasurements(deviceId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetMinimumValuesByDateAsync_ShouldReturnOkWithValues()
    {
        const int deviceId = 1;
        var expected = new List<MinMeasurmentValueAtDate>
        {
            new() { AtDate = new DateTime(2026, 1, 15, 8, 0, 0, DateTimeKind.Utc), MinValue = 7.5 }
        };

        A.CallTo(() => _monitoringService.CalculateMinValue(deviceId))
            .Returns(Task.FromResult<IEnumerable<MinMeasurmentValueAtDate>>(expected));

        var result = await _controller.GetMinimumValuesByDateAsync(deviceId);

        AssertOk(result, expected);

        A.CallTo(() => _monitoringService.CalculateMinValue(deviceId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetMaximumValuesByDateAsync_ShouldReturnOkWithValues()
    {
        const int deviceId = 1;
        var expected = new List<MaxMeasurmentValueAtDate>
        {
            new() { AtDate = new DateTime(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc), MaxValue = 22.3 }
        };

        A.CallTo(() => _monitoringService.CalculateMaxValue(deviceId))
            .Returns(Task.FromResult<IEnumerable<MaxMeasurmentValueAtDate>>(expected));

        var result = await _controller.GetMaximumValuesByDateAsync(deviceId);

        AssertOk(result, expected);

        A.CallTo(() => _monitoringService.CalculateMaxValue(deviceId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAverageValueAsync_ShouldReturnOkWithAverage()
    {
        const int deviceId = 1;
        double? expected = 12.5;

        A.CallTo(() => _monitoringService.CalculateAverageValue(deviceId))
            .Returns(Task.FromResult(expected));

        var result = await _controller.GetAverageValueAsync(deviceId);

        AssertOk(result, expected);

        A.CallTo(() => _monitoringService.CalculateAverageValue(deviceId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetMeasurementCountAboveThresholdAsync_ShouldReturnOkWithCount()
    {
        const int deviceId = 1;
        const double threshold = 15.0;
        const int expectedCount = 2;

        A.CallTo(() => _monitoringService.GetMeasurementCountAboveThreshold(deviceId, threshold))
            .Returns(Task.FromResult(expectedCount));

        var result = await _controller.GetMeasurementCountAboveThresholdAsync(deviceId, threshold);

        AssertOk(result, expectedCount);

        A.CallTo(() => _monitoringService.GetMeasurementCountAboveThreshold(deviceId, threshold))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetMeasurementCountBelowThresholdAsync_ShouldReturnOkWithCount()
    {
        const int deviceId = 1;
        const double threshold = 10.0;
        const int expectedCount = 3;

        A.CallTo(() => _monitoringService.GetMeasurementCountBelowThreshold(deviceId, threshold))
            .Returns(Task.FromResult(expectedCount));

        var result = await _controller.GetMeasurementCountBelowThresholdAsync(deviceId, threshold);

        AssertOk(result, expectedCount);

        A.CallTo(() => _monitoringService.GetMeasurementCountBelowThreshold(deviceId, threshold))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAverageValueForDateRangeAsync_ShouldReturnOkWithAverage()
    {
        const int deviceId = 1;
        var startDate = new DateTime(2026, 1, 1);
        var endDate = new DateTime(2026, 1, 31);
        double? expected = 11.8;

        A.CallTo(() => _monitoringService.CalculateAverageValueForDateRange(deviceId, startDate, endDate))
            .Returns(Task.FromResult(expected));

        var result = await _controller.GetAverageValueForDateRangeAsync(deviceId, startDate, endDate);

        AssertOk(result, expected);

        A.CallTo(() => _monitoringService.CalculateAverageValueForDateRange(deviceId, startDate, endDate))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetMinimumValueForDateRangeAsync_ShouldReturnOkWithValue()
    {
        const int deviceId = 1;
        var startDate = new DateTime(2026, 1, 1);
        var endDate = new DateTime(2026, 1, 31);
        double? expected = 7.5;

        A.CallTo(() => _monitoringService.CalculateMinValueForDateRange(deviceId, startDate, endDate))
            .Returns(Task.FromResult(expected));

        var result = await _controller.GetMinimumValueForDateRangeAsync(deviceId, startDate, endDate);

        AssertOk(result, expected);

        A.CallTo(() => _monitoringService.CalculateMinValueForDateRange(deviceId, startDate, endDate))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetMaximumValueForDateRangeAsync_ShouldReturnOkWithValue()
    {
        const int deviceId = 1;
        var startDate = new DateTime(2026, 1, 1);
        var endDate = new DateTime(2026, 1, 31);
        double? expected = 22.3;

        A.CallTo(() => _monitoringService.CalculateMaxValueForDateRange(deviceId, startDate, endDate))
            .Returns(Task.FromResult(expected));

        var result = await _controller.GetMaximumValueForDateRangeAsync(deviceId, startDate, endDate);

        AssertOk(result, expected);

        A.CallTo(() => _monitoringService.CalculateMaxValueForDateRange(deviceId, startDate, endDate))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetMeasurementsForDateRangeAsync_ShouldReturnOkWithMeasurements()
    {
        const int deviceId = 1;
        var startDate = new DateTime(2026, 1, 1);
        var endDate = new DateTime(2026, 1, 31);

        var expected = new List<Measurement>
        {
            new() { Id = 1, DeviceId = deviceId, Value = 10.5, CreatedOn = new DateTime(2026, 1, 10, 9, 0, 0, DateTimeKind.Utc) }
        };

        A.CallTo(() => _monitoringService.GetMeasurementsForDateRange(deviceId, startDate, endDate))
            .Returns(Task.FromResult<IEnumerable<Measurement>>(expected));

        var result = await _controller.GetMeasurementsForDateRangeAsync(deviceId, startDate, endDate);

        AssertOk(result, expected);

        A.CallTo(() => _monitoringService.GetMeasurementsForDateRange(deviceId, startDate, endDate))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetLatestMeasurementsAsync_ShouldReturnOkWithMeasurements()
    {
        const int deviceId = 1;
        const int count = 5;

        var expected = new List<Measurement>
        {
            new() { Id = 3, DeviceId = deviceId, Value = 15.2, CreatedOn = new DateTime(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc) }
        };

        A.CallTo(() => _monitoringService.GetLatestMeasurements(deviceId, count))
            .Returns(Task.FromResult<IEnumerable<Measurement>>(expected));

        var result = await _controller.GetLatestMeasurementsAsync(deviceId, count);

        AssertOk(result, expected);

        A.CallTo(() => _monitoringService.GetLatestMeasurements(deviceId, count))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetHourlySummaryAsync_ShouldReturnOkWithSummary()
    {
        const int deviceId = 1;
        var date = new DateTime(2026, 1, 15);

        var expected = new List<NumberAndAverageValueInHour>
        {
            new() { AtHour = 9, NumberOfMeasurements = 2, AverageValue = 11.4 }
        };

        A.CallTo(() => _monitoringService.GetNumberAndAverageValueInHourByDate(deviceId, date))
            .Returns(Task.FromResult<IEnumerable<NumberAndAverageValueInHour>>(expected));

        var result = await _controller.GetHourlySummaryAsync(deviceId, date);

        AssertOk(result, expected);

        A.CallTo(() => _monitoringService.GetNumberAndAverageValueInHourByDate(deviceId, date))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetTotalMeasurementCountAsync_ShouldPropagateServiceException()
    {
        const int deviceId = 999;

        A.CallTo(() => _monitoringService.GetTotalMeasurementCount(deviceId))
            .ThrowsAsync(new ResourceNotFoundException("Device", deviceId));

        Func<Task> action = async () => await _controller.GetTotalMeasurementCountAsync(deviceId);

        await action.Should().ThrowAsync<ResourceNotFoundException>();
    }

    private static void AssertOk<T>(ActionResult<T> result, T expected)
    {
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(expected);
    }
}