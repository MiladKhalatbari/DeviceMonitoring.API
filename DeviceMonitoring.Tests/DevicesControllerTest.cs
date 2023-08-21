using DeviceMonitoring.API.Controllers;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.Business;
using DeviceMonitoring.Services.DataTransferObjects.Devices;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitoring.Tests
{
    public class DevicesControllerTest
    {
        private IDeviceService _mockDeviceService;
        private ILogger<DevicesController> _mockLogger;
        private DevicesController _controller;


        public DevicesControllerTest()
        {
            _mockDeviceService = A.Fake<IDeviceService>();
            _mockLogger = A.Fake<ILogger<DevicesController>>();
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithDevices()
        {
            // Arrange
            var fakeDevices = A.Fake<ICollection<Device>>();
            A.CallTo(() => _mockDeviceService.GetAllAsync()).Returns(fakeDevices);
            _controller = new DevicesController(_mockDeviceService, _mockLogger);

            // Act
            var result = await _controller.Get();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<Device>>>();
            var okResult = result.Should().BeOfType<ActionResult<IEnumerable<Device>>>().Subject.Result;
            okResult.Should().BeOfType<OkObjectResult>();
            var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
            okObjectResult.Value.Should().BeEquivalentTo(fakeDevices);
        }

        [Fact]
        public async Task GetById_ReturnsOkResultWithDevice()
        {
            // Arrange
            int deviceId = 1;
            var fakeDevice = A.Fake<Device>();
            A.CallTo(() => _mockDeviceService.GetByIdAsync(deviceId)).Returns(fakeDevice);
            _controller = new DevicesController(_mockDeviceService, _mockLogger);

            // Act
            var result = await _controller.GetById(deviceId);

            // Assert
            result.Should().BeOfType<ActionResult<Device>>();
            var okResult = result.Should().BeOfType<ActionResult<Device>>().Subject.Result;
            okResult.Should().BeOfType<OkObjectResult>();
            var okObjectResult = okResult.Should().BeOfType<OkObjectResult>().Subject;
            okObjectResult.Value.Should().BeEquivalentTo(fakeDevice);
        }

        [Fact]
        public async Task CreateDivice_ReturnOkResult()
        {
            //Arrange
           var fakeDevice = A.Fake<DeviceForInsertDto>();
            int fakeId = 1;
            A.CallTo(() => _mockDeviceService.AddAsync(fakeDevice)).Returns(fakeId);
            _controller = new DevicesController( _mockDeviceService, _mockLogger);
            //Act
           var result = await _controller.Create(fakeDevice);
            //Assert
            result.Should().BeOfType<OkResult>();
        }
        [Fact]
        public async Task UpdateDivice_ReturnOkResult()
        {
            //Arrange
            var fakeDevice = A.Fake<DeviceForUpdateDto>();
            A.CallTo(() => _mockDeviceService.UpdateAsync(fakeDevice)).Returns(fakeDevice.Id);
            _controller = new DevicesController(_mockDeviceService, _mockLogger);
            //Act
            var result = await _controller.Update(fakeDevice.Id,fakeDevice);
            //Assert
            result.Should().BeOfType<OkResult>();
        }
        [Fact]
        public async Task DeleteDivice_ReturnOkResult()
        {
            //Arrange
            var fakeDevice = A.Fake<Device>();
            A.CallTo(() => _mockDeviceService.DeleteAsync(fakeDevice.Id)).Returns(fakeDevice.Id);
            _controller = new DevicesController(_mockDeviceService, _mockLogger);
            //Act
            var result = await _controller.Delete(fakeDevice.Id);
            //Assert
            result.Should().BeOfType<OkResult>();
        }
    }
}