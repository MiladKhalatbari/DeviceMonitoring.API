using DeviceMonitoring.API.Controllers;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.Business;
using DeviceMonitoring.Services.DataTransferObjects.Devices;
using DeviceMonitoring.Services.DataTransferObjects.Measurments;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitoring.Tests;
public class DataControllerTest
{
    private readonly IDeviceService _deviceService;
    private readonly IMeasurementService _measurementService;
    private readonly ILogger<DataController> _logger;
    private DataController? _controller;
    public DataControllerTest()
    {
        _deviceService = A.Fake<IDeviceService>();
        _measurementService = A.Fake<IMeasurementService>();
        _logger = A.Fake<ILogger<DataController>>();
    }

    [Fact]
    public async Task CreateDevice_AddMeasurment_returnOkResult()
    {
        // Arrange
        var deviceName = "Device 6";
        var value = 10.0;
        int fakeId = 5;
        var fakeDevice = A.Fake<Device>();
        A.CallTo(() =>  _deviceService.GetByNameAsync(deviceName)).Returns(fakeDevice);
        MeasurementForInsertDto fakeMeasurement = new MeasurementForInsertDto { Value = value, DeviceId = fakeDevice.Id };
        A.CallTo(() =>  _measurementService.AddAsync(fakeMeasurement)).Returns(fakeId);
        _controller = new DataController(_deviceService, _measurementService, _logger);

        // Act
        var result = await _controller.Listen(deviceName, value);

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}
