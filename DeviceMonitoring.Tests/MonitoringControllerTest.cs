using Castle.Core.Logging;
using DeviceMonitoring.API.Controllers;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.Business;
using DeviceMonitoring.Services.DataTransferObjects;
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
public class MonitoringControllerTest
{
    private readonly IMonitoringService _monitoringService;
    private readonly ILogger<MonitoringController> _logger;
    private MonitoringController? _controller;
    public MonitoringControllerTest()
    {
        _logger = A.Fake<ILogger<MonitoringController>>();
        _monitoringService = A.Fake<IMonitoringService>();
    }

    [Fact]
    public async Task GetMeasurments_Count_ReturnOkObjectResult()
    {
        //Arrange
        var deviceId = A.Fake<Device>().Id;
        int fakeResult = 1;
        A.CallTo(() => _monitoringService.GetTotalMeasurementCount(deviceId)).Returns(fakeResult);
        _controller = new MonitoringController(_monitoringService, _logger);
        //Act
        var result =await _controller.GetTotalMeasurementCount(deviceId);
        //Assert
        result.Should().BeOfType<ActionResult<int>>();
        var okResult = result.Should().BeOfType< ActionResult<int>> ().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
        okObjectResult.Value.Should().BeEquivalentTo(fakeResult);
    }

    [Fact]
    public async Task GetDeviceName_And_NumberOfMeasurements_ReturnOkObjectResult()
    {
        //Arrange
        var deviceId = A.Fake<Device>().Id;
        var deviceNameAndNumberOfMeasurements = A.Fake<IEnumerable<DeviceNameAndNumberOfMeasurements>>();
        A.CallTo(() => _monitoringService.GetDeviceNameAndNumberOfMeasurements(deviceId)).Returns(deviceNameAndNumberOfMeasurements);
        _controller = new MonitoringController(_monitoringService, _logger);
        //Act
        var result =await _controller.GetDeviceNameAndNumberOfMeasurements(deviceId);
        //Assert
        result.Should().BeOfType<ActionResult<IEnumerable<DeviceNameAndNumberOfMeasurements>>>();
        var okResult = result.Should().BeOfType<ActionResult<IEnumerable<DeviceNameAndNumberOfMeasurements>>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
        okObjectResult.Value.Should().BeEquivalentTo(deviceNameAndNumberOfMeasurements);

    }
    [Fact]
    public async Task Get_MinValues_ReturnOkObjectResult()
    {
        //Arrange
        var deviceId = A.Fake<Device>().Id;
        var minMeasurmentValueAtDate = A.Fake<IEnumerable<MinMeasurmentValueAtDate>>();
        A.CallTo(() => _monitoringService.CalculateMinValue(deviceId)).Returns(minMeasurmentValueAtDate);
        _controller = new MonitoringController(_monitoringService, _logger);
        //Act
        var result =await _controller.CalculateMinValue(deviceId);
        //Assert
        result.Should().BeOfType<ActionResult<IEnumerable<MinMeasurmentValueAtDate>>>();
        var okResult = result.Should().BeOfType<ActionResult<IEnumerable<MinMeasurmentValueAtDate>>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
       okObjectResult.Value.Should().BeEquivalentTo(minMeasurmentValueAtDate);

    }
    [Fact]

    public async Task Get_MaxValues_ReturnOkObjectResult()
    {
        //Arrange
        var deviceId = A.Fake<Device>().Id;
        var maxMeasurmentValueAtDate = A.Fake<IEnumerable<MaxMeasurmentValueAtDate>>();
        A.CallTo(() => _monitoringService.CalculateMaxValue(deviceId)).Returns(maxMeasurmentValueAtDate);
        _controller = new MonitoringController(_monitoringService, _logger);
        //Act
        var result =await _controller.CalculateMaxValue(deviceId);
        //Assert
        result.Should().BeOfType<ActionResult<IEnumerable<MaxMeasurmentValueAtDate>>>();
        var okResult = result.Should().BeOfType<ActionResult<IEnumerable<MaxMeasurmentValueAtDate>>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
       okObjectResult.Value.Should().BeEquivalentTo(maxMeasurmentValueAtDate);

    }
    [Fact]
    public async Task Get_AverageValue_ReturnOkObjectResult()
    {
        //Arrange
        var deviceId = A.Fake<Device>().Id;
        double fakeReturn = 6.5;
       A.CallTo(() => _monitoringService.CalculateAverageValue(deviceId)).Returns(fakeReturn);
        _controller = new MonitoringController(_monitoringService, _logger);
        //Act
        var result = await _controller.CalculateAverageValue(deviceId);
        //Assert
        result.Should().BeOfType<ActionResult<double>>();
        var okResult = result.Should().BeOfType<ActionResult<double>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
       okObjectResult.Value.Should().BeEquivalentTo(fakeReturn);

    }
    [Fact]
    public async Task GetMeasurementCount_Above_Threshold_ReturnOkObjectResult()
    {
        //Arrange
        var deviceId = A.Fake<Device>().Id;
        int fakeResult = 1;
        int threshold = 2;

        A.CallTo(() => _monitoringService.GetMeasurementCountAboveThreshold(deviceId, threshold)).Returns(fakeResult);
        _controller = new MonitoringController(_monitoringService, _logger);
        //Act
        var result = await _controller.GetMeasurementCountAboveThreshold(deviceId,threshold);
        //Assert
        result.Should().BeOfType<ActionResult<int>>();
        var okResult = result.Should().BeOfType<ActionResult<int>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
        okObjectResult.Value.Should().BeEquivalentTo(fakeResult);
    }
    [Fact]
    public async Task GetMeasurementCount_Below_Threshold_ReturnOkObjectResult()
    {
        //Arrange
        var deviceId = A.Fake<Device>().Id;
        int fakeResult = 1;
        int threshold = 2;

        A.CallTo(() => _monitoringService.GetMeasurementCountBelowThreshold(deviceId, threshold)).Returns(fakeResult);
        _controller = new MonitoringController(_monitoringService, _logger);
        //Act
        var result = await _controller.GetMeasurementCountBelowThreshold(deviceId, threshold);
        //Assert
        result.Should().BeOfType<ActionResult<int>>();
        var okResult = result.Should().BeOfType<ActionResult<int>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
        okObjectResult.Value.Should().BeEquivalentTo(fakeResult);
    }
    [Fact]
    public async Task AverageValue_ForDateRange_ReturnOkObjectResult()
    {
        //Arrange
        var deviceId = A.Fake<Device>().Id;
        double fakeReturn = 6.5;
        DateTime fakeStartDate = DateTime.Now.AddDays(-100);
        DateTime fakeEndDate = DateTime.Now;

        A.CallTo(() => _monitoringService.CalculateAverageValueForDateRange(deviceId,fakeStartDate,fakeEndDate)).Returns(fakeReturn);
        _controller = new MonitoringController(_monitoringService, _logger);
        //Act
        var result = await _controller.CalculateAverageValueForDateRange(deviceId, fakeStartDate, fakeEndDate);
        //Assert
        result.Should().BeOfType<ActionResult<double>>();
        var okResult = result.Should().BeOfType<ActionResult<double>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
        okObjectResult.Value.Should().BeEquivalentTo(fakeReturn);

    }
    [Fact]
    public async Task MinValue_ForDateRange_ReturnOkObjectResult()
    {
        //Arrange
        var deviceId = A.Fake<Device>().Id;
        double fakeReturn = 6.5;
        DateTime fakeStartDate = DateTime.Now.AddDays(-100);
        DateTime fakeEndDate = DateTime.Now;

        A.CallTo(() => _monitoringService.CalculateMinValueForDateRange(deviceId, fakeStartDate, fakeEndDate)).Returns(fakeReturn);
        _controller = new MonitoringController(_monitoringService, _logger);
        //Act
        var result = await _controller.CalculateMinValueForDateRange(deviceId, fakeStartDate, fakeEndDate);
        //Assert
        result.Should().BeOfType<ActionResult<double>>();
        var okResult = result.Should().BeOfType<ActionResult<double>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
       okObjectResult.Value.Should().BeEquivalentTo(fakeReturn);

    }
    [Fact]
    public async Task MaxValue_ForDateRange_ReturnOkObjectResult()
    {
        //Arrange
        var deviceId = A.Fake<Device>().Id;
        double fakeReturn = 6.5;
        DateTime fakeStartDate = DateTime.Now.AddDays(-100);
        DateTime fakeEndDate = DateTime.Now;

        A.CallTo(() => _monitoringService.CalculateMaxValueForDateRange(deviceId, fakeStartDate, fakeEndDate)).Returns(fakeReturn);
        _controller = new MonitoringController(_monitoringService, _logger);
        //Act
        var result = await _controller.CalculateMaxValueForDateRange(deviceId, fakeStartDate, fakeEndDate);
        //Assert
        result.Should().BeOfType<ActionResult<double>>();
        var okResult = result.Should().BeOfType<ActionResult<double>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
        okObjectResult.Value.Should().BeEquivalentTo(fakeReturn);

    }

    [Fact]
    public async Task GetMeasurements_ForDateRange_ReturnOkObjectResult()
    {
        //Arrange
        var deviceId = A.Fake<Device>().Id;
        DateTime fakeStartDate = DateTime.Now.AddDays(-100);
        DateTime fakeEndDate = DateTime.Now;
        var measurements = A.Fake<IEnumerable<Measurement>>();
        A.CallTo(() => _monitoringService.GetMeasurementsForDateRange(deviceId,fakeStartDate,fakeEndDate)).Returns(measurements);
        _controller = new MonitoringController(_monitoringService, _logger);
        //Act
        var result = await _controller.GetMeasurementsForDateRange(deviceId,fakeStartDate,fakeEndDate);
        //Assert
        result.Should().BeOfType<ActionResult<IEnumerable<Measurement>>>();
        var okResult = result.Should().BeOfType<ActionResult<IEnumerable<Measurement>>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
        okObjectResult.Value.Should().BeEquivalentTo(measurements);

    }
    [Fact]
    public async Task Get_LatestMeasurements_ReturnOkObjectResult()
    {
        //Arrange
        var deviceId = A.Fake<Device>().Id;
        int fakeCount = 4;
        var deviceNameAndNumberOfMeasurements = A.Fake<IEnumerable<Measurement>>();
        A.CallTo(() => _monitoringService.GetLatestMeasurements(deviceId,fakeCount)).Returns(deviceNameAndNumberOfMeasurements);
        _controller = new MonitoringController(_monitoringService, _logger);
        //Act
        var result = await _controller.GetLatestMeasurements(deviceId, fakeCount);
        //Assert
        result.Should().BeOfType<ActionResult<IEnumerable<Measurement>>>();
        var okResult = result.Should().BeOfType<ActionResult<IEnumerable<Measurement>>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
       okObjectResult.Value.Should().BeEquivalentTo(deviceNameAndNumberOfMeasurements);

    }
    [Fact]
    public async Task Get_Number_Average_Value_InHourByDate_ReturnOkObjectResult()
    {
        //Arrange
        var deviceId = A.Fake<Device>().Id;
        DateTime fakeDate =new DateTime(2023,08,20);

        var numberAndAverageValueInHour = A.Fake<IEnumerable<NumberAndAverageValueInHour>>();
        A.CallTo(() => _monitoringService.GetNumberAndAverageValueInHourByDate(deviceId,fakeDate)).Returns(numberAndAverageValueInHour);
        _controller = new MonitoringController(_monitoringService, _logger);
        //Act
        var result = await _controller.GetNumberAndAverageValueInHourByDate(deviceId, fakeDate);
        //Assert
        result.Should().BeOfType<ActionResult<IEnumerable<NumberAndAverageValueInHour>>>();
        var okResult = result.Should().BeOfType<ActionResult<IEnumerable<NumberAndAverageValueInHour>>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
        okObjectResult.Value.Should().BeEquivalentTo(numberAndAverageValueInHour);

    }
}
