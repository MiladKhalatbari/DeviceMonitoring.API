using DeviceMonitoring.API.Controllers;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.Devices;
using DeviceMonitoring.Services.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.Tests;

public sealed class DevicesControllerTests
{
    private readonly IDeviceService _deviceService;
    private readonly DevicesController _controller;

    public DevicesControllerTests()
    {
        _deviceService = A.Fake<IDeviceService>();
        _controller = new DevicesController(_deviceService);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOkWithDevicesAsync()
    {
        // Arrange
        var devices = new List<Device>
        {
            new() { Id = 1, Name = "Device 1", Location = "Prague" },
            new() { Id = 2, Name = "Device 2", Location = "Brno" }
        };

        A.CallTo(() => _deviceService.GetAllAsync())
            .Returns(Task.FromResult<IEnumerable<Device>>(devices));

        // Act
        var result = await _controller.GetAllAsync();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(devices);

        A.CallTo(() => _deviceService.GetAllAsync())
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOkWithDeviceAsync()
    {
        // Arrange
        const int deviceId = 1;

        var device = new Device
        {
            Id = deviceId,
            Name = "Device 1",
            Location = "Prague"
        };

        A.CallTo(() => _deviceService.GetByIdAsync(deviceId))
            .Returns(Task.FromResult(device));

        // Act
        var result = await _controller.GetByIdAsync(deviceId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(device);

        A.CallTo(() => _deviceService.GetByIdAsync(deviceId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedAtRouteAsync()
    {
        // Arrange
        const int createdDeviceId = 1;

        var request = new DeviceForInsertDto
        {
            Name = "Device 1",
            Location = "Prague",
            Manufacturer = "Example Manufacturer",
            ModelNumber = "Model-001"
        };

        A.CallTo(() => _deviceService.AddAsync(request))
            .Returns(Task.FromResult(createdDeviceId));

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtRouteResult>().Subject;

        createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdResult.RouteName.Should().Be("GetDeviceById");
        createdResult.RouteValues!["id"].Should().Be(createdDeviceId);
        createdResult.Value.Should().BeEquivalentTo(new { id = createdDeviceId });

        A.CallTo(() => _deviceService.AddAsync(request))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNoContentAsync()
    {
        // Arrange
        const int deviceId = 1;

        var request = new DeviceForUpdateDto
        {
            Id = deviceId,
            Location = "Updated Prague location",
            IsActive = true
        };

        A.CallTo(() => _deviceService.UpdateAsync(request))
            .Returns(Task.FromResult(deviceId));

        // Act
        var result = await _controller.UpdateAsync(deviceId, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        A.CallTo(() => _deviceService.UpdateAsync(request))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContentAsync()
    {
        // Arrange
        const int deviceId = 1;

        A.CallTo(() => _deviceService.DeleteAsync(deviceId))
            .Returns(Task.FromResult(deviceId));

        // Act
        var result = await _controller.DeleteAsync(deviceId);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        A.CallTo(() => _deviceService.DeleteAsync(deviceId))
            .MustHaveHappenedOnceExactly();
    }
}