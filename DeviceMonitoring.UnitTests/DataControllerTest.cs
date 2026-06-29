using DeviceMonitoring.API.Controllers;
using DeviceMonitoring.Services.DataTransferObjects.Measurements;
using DeviceMonitoring.Services.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.Tests;

public sealed class DataControllerTests
{
    private readonly IDataIngestionService _dataIngestionService;
    private readonly DataController _controller;

    public DataControllerTests()
    {
        _dataIngestionService = A.Fake<IDataIngestionService>();
        _controller = new DataController(_dataIngestionService);
    }

    [Fact]
    public async Task IngestAsync_ShouldReturn201Created_WithIngestionResponse()
    {
        // Arrange
        var request = new MeasurementIngestionRequestDto
        {
            DeviceName = "Device 6",
            Value = 10.0
        };

        var expectedResponse = new MeasurementIngestionResponseDto(
            DeviceId: 6,
            DeviceCreated: true);

        A.CallTo(() => _dataIngestionService.IngestAsync(request))
            .Returns(expectedResponse);

        // Act
        var result = await _controller.IngestAsync(request);

        // Assert
        var objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;

        objectResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        objectResult.Value.Should().BeEquivalentTo(expectedResponse);

        A.CallTo(() => _dataIngestionService.IngestAsync(request))
            .MustHaveHappenedOnceExactly();
    }
}