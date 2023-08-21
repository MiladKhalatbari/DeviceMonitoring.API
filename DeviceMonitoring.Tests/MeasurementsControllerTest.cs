using DeviceMonitoring.API.Controllers;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.Business;
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
public class MeasurementsControllerTest
{

    private readonly IMeasurementService _fakeMeasurementService;
    private readonly ILogger<MeasurementsController> _fakeLogger;
    private MeasurementsController? _controller;

    public MeasurementsControllerTest()
    {
        _fakeLogger = A.Fake<ILogger<MeasurementsController>>();
        _fakeMeasurementService = A.Fake<IMeasurementService>();
    }

    [Fact]
    public async Task Get_ReturnOkResultWithMeasurments()
    {
        //Arrange
        var fakeMeasurement = A.Fake<IEnumerable<Measurement>>();
        A.CallTo(() => _fakeMeasurementService.GetAllAsync()).Returns(fakeMeasurement);
        _controller = new MeasurementsController(_fakeMeasurementService, _fakeLogger);
        //Act
        var result = await _controller.Get();
        //Assert

        result.Should().BeOfType<ActionResult<IEnumerable<Measurement>>>();
        var okResult = result.Should().BeOfType<ActionResult<IEnumerable<Measurement>>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjcetResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
        okObjcetResult.Value.Should().BeEquivalentTo(fakeMeasurement);
    }

    [Fact]
    public async Task GetById_ReturnOkResultWithMeasurment()
    {
        //Arrange
        var fakeMeasurment = A.Fake<Measurement>();
        A.CallTo(() => _fakeMeasurementService.GetByIdAsync(fakeMeasurment.Id)).Returns(fakeMeasurment);
        _controller = new MeasurementsController(_fakeMeasurementService, _fakeLogger);
        //Act
        var result = await _controller.GetById(fakeMeasurment.Id);

        //Assert
        result.Should().BeOfType<ActionResult<Measurement>>();
        var okResult = result.Should().BeOfType<ActionResult<Measurement>>().Subject.Result;
        okResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
        okObjectResult.Value.Should().BeEquivalentTo(fakeMeasurment);
    }

    [Fact]
    public async Task CreateMesurment_ReturnOkResult()
    {
        //Arrange
       var fakeMesurment = A.Fake<MeasurementForInsertDto>();
        int fakeId = 5;
        A.CallTo(() => _fakeMeasurementService.AddAsync(fakeMesurment)).Returns(fakeId);
        _controller = new MeasurementsController( _fakeMeasurementService, _fakeLogger);
        //Act
         var result = await _controller.Create(fakeMesurment);
        //Assert
        result.Should().BeOfType<OkResult>();
    }
    [Fact]
    public async Task UpdateMesurment_ReturnOkResult()
    {
        //Arrange
        var fakeMesurment = A.Fake<MeasurementForUpdateDto>();
        A.CallTo(() => _fakeMeasurementService.UpdateAsync(fakeMesurment)).Returns(fakeMesurment.Id);
        _controller = new MeasurementsController(_fakeMeasurementService, _fakeLogger);
        //Act
        var result = await _controller.Update(fakeMesurment.Id,fakeMesurment);
        //Assert
        result.Should().BeOfType<OkResult>();
    }
    [Fact]
    public async Task DeleteMesurment_ReturnOkResult()
    {
        //Arrange
        var fakeMesurment = A.Fake<Measurement>();
        A.CallTo(() => _fakeMeasurementService.DeleteAsync(fakeMesurment.Id)).Returns(fakeMesurment.Id);
        _controller = new MeasurementsController(_fakeMeasurementService, _fakeLogger);
        //Act
        var result = await _controller.Delete(fakeMesurment.Id);
        //Assert
        result.Should().BeOfType<OkResult>();
    }
}
