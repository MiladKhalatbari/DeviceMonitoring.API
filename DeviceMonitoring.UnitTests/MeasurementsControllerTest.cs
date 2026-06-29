using DeviceMonitoring.API.Controllers;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.Measurements;
using DeviceMonitoring.Services.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.Tests;

public sealed class MeasurementsControllerTests
{
    private readonly IMeasurementService _measurementService;
    private readonly MeasurementsController _controller;

    public MeasurementsControllerTests()
    {
        _measurementService = A.Fake<IMeasurementService>();
        _controller = new MeasurementsController(_measurementService);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOkWithMeasurements()
    {
        // Arrange
        var measurements = new List<Measurement>
        {
            new() { Id = 1, DeviceId = 1, Value = 10.5, CreatedOn = DateTime.UtcNow },
            new() { Id = 2, DeviceId = 1, Value = 12.3, CreatedOn = DateTime.UtcNow }
        };

        A.CallTo(() => _measurementService.GetAllAsync())
            .Returns(Task.FromResult<IEnumerable<Measurement>>(measurements));

        // Act
        var result = await _controller.GetAllAsync();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(measurements);

        A.CallTo(() => _measurementService.GetAllAsync())
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOkWithMeasurement()
    {
        // Arrange
        const int measurementId = 1;

        var measurement = new Measurement
        {
            Id = measurementId,
            DeviceId = 1,
            Value = 10.5,
            CreatedOn = DateTime.UtcNow
        };

        A.CallTo(() => _measurementService.GetByIdAsync(measurementId))
            .Returns(Task.FromResult(measurement));

        // Act
        var result = await _controller.GetByIdAsync(measurementId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(measurement);

        A.CallTo(() => _measurementService.GetByIdAsync(measurementId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedAtRoute()
    {
        // Arrange
        const int createdMeasurementId = 5;

        var request = new MeasurementForInsertDto
        {
            DeviceId = 1,
            Value = 10.5
        };

        A.CallTo(() => _measurementService.AddAsync(request))
            .Returns(Task.FromResult(createdMeasurementId));

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtRouteResult>().Subject;

        createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdResult.RouteName.Should().Be("GetMeasurementById");
        createdResult.RouteValues!["id"].Should().Be(createdMeasurementId);
        createdResult.Value.Should().BeEquivalentTo(new { id = createdMeasurementId });

        A.CallTo(() => _measurementService.AddAsync(request))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNoContent()
    {
        // Arrange
        const int measurementId = 1;

        var request = new MeasurementForUpdateDto
        {
            Id = measurementId,
            Value = 15.2
        };

        A.CallTo(() => _measurementService.UpdateAsync(request))
            .Returns(Task.FromResult(measurementId));

        // Act
        var result = await _controller.UpdateAsync(measurementId, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        A.CallTo(() => _measurementService.UpdateAsync(request))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowArgumentException_WhenRouteIdDoesNotMatchRequestId()
    {
        // Arrange
        var request = new MeasurementForUpdateDto
        {
            Id = 2,
            Value = 15.2
        };

        // Act
        var action = async () => await _controller.UpdateAsync(1, request);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();

        A.CallTo(() => _measurementService.UpdateAsync(A<MeasurementForUpdateDto>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent()
    {
        // Arrange
        const int measurementId = 1;

        A.CallTo(() => _measurementService.DeleteAsync(measurementId))
            .Returns(Task.FromResult(measurementId));

        // Act
        var result = await _controller.DeleteAsync(measurementId);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        A.CallTo(() => _measurementService.DeleteAsync(measurementId))
            .MustHaveHappenedOnceExactly();
    }
}