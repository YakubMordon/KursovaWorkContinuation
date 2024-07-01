using FluentAssertions;
using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Domain.Entities.Car;
using KursovaWork.UI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Newtonsoft.Json.Linq;

namespace KursovaWork.UI.Unit.Tests.Controllers;

public class ConfiguratorControllerTests
{
    private readonly Mock<ICarService> _mockCarService;
    private readonly ConfiguratorController _controller;

    public ConfiguratorControllerTests()
    {
        _mockCarService = new Mock<ICarService>();
        _controller = new ConfiguratorController(_mockCarService.Object);
    }

    [Theory]
    [InlineData("Make", "Model", "2023")]
    public void Configurator_ValidParameters_ShouldReturnView(string param1, string param2, string param3)
    {
        // Arrange
        var year = int.Parse(param3);
        var car = new Car { Make = param1, Model = param2, Year = year };
        _mockCarService.Setup(s => s.GetCarByInfo(param1, param2, year)).Returns(car);

        // Act
        var result = _controller.Configurator(param1, param2, param3);

        // Assert
        var viewResult = result as ViewResult;
        viewResult.Should().NotBeNull();
        viewResult!.Model.Should().Be(car);
    }

    [Theory]
    [InlineData("", "Model", "2023")]
    [InlineData("Make", "", "2023")]
    public void Configurator_InvalidParameters_NotNumerical_ShouldReturnErrorView(string param1, string param2, string param3)
    {
        // Act
        var result = _controller.Configurator(param1, param2, param3);

        // Assert
        var viewResult = result as ViewResult;
        viewResult.Should().NotBeNull();
        viewResult!.ViewName.Should().Be(null);
    }

    [Fact]
    public void Configurator_InvalidParameter_WithNumerical_ShouldThrowFormatException()
    {
        // Act
        Action configure = () => _controller.Configurator("Make", "Model", "");

        // Assert
        configure.Should().Throw<FormatException>();
    }

    [Theory]
    [InlineData("Red", "Automatic", "Petrol")]
    public void Submit_ValidParameters_ShouldReturnJsonWithRedirect(string color, string transmission, string fuelType)
    {
        // Arrange
        _controller.Configurator("Make", "Model", "2023");

        // Mock IUrlHelper
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
            .Returns("/Payment/Payment");

        _controller.Url = mockUrlHelper.Object;

        // Act
        var result = _controller.Submit(color, transmission, fuelType);

        // Assert
        var jsonResult = result as JsonResult;
        jsonResult.Should().NotBeNull();

        var jsonData = JObject.FromObject(jsonResult!.Value);
        jsonData["redirect"].Should().NotBeNull();

        var redirectUrl = (string)jsonData["redirect"];
        redirectUrl.Should().Contain("/Payment/Payment");
    }

    [Theory]
    [InlineData("", "Automatic", "Petrol")]
    [InlineData("Red", "", "Petrol")]
    [InlineData("Red", "Automatic", "")]
    public void Submit_IncompleteParameters_ShouldReturnJsonWithErrors(string color, string transmission, string fuelType)
    {
        // Arrange
        _controller.Configurator("Make", "Model", "2023");

        // Act
        var result = _controller.Submit(color, transmission, fuelType);

        // Assert
        var jsonResult = result as JsonResult;
        jsonResult.Should().NotBeNull();

        var jsonData = JObject.FromObject(jsonResult!.Value);
        jsonData["errors"].Should().NotBeNull();

        var errors = jsonData["errors"].ToObject<Dictionary<string, string>>();
        errors.Should().NotBeNullOrEmpty();
    }
}