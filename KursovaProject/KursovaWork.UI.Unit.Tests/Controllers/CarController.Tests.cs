using FluentAssertions;
using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Domain.Entities.Car;
using KursovaWork.UI.Controllers;
using KursovaWork.UI.Unit.Tests.Fakers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KursovaWork.UI.Unit.Tests.Controllers;

public class CarControllerTests
{
    private readonly Mock<ICarService> _mockCarService;
    private readonly CarController _controller;
    private readonly CarFaker _carFaker;

    public CarControllerTests()
    {
        _mockCarService = new Mock<ICarService>();
        _controller = new CarController(_mockCarService.Object);

        _carFaker = new CarFaker();
    }

    [Fact]
    public void Car_InvalidYearParameter_ShouldReturnErrorView()
    {
        // Act
        var result = _controller.Car("Make", "Model", "InvalidYear");

        // Assert
        var viewResult = result as ViewResult;

        viewResult.Should().NotBeNull();
        viewResult!.ViewName.Should().Be("Error");
    }

    [Fact]
    public void Car_CarFound_ShouldReturnCarView()
    {
        // Arrange
        var car = _carFaker.Generate();
        _mockCarService.Setup(service => service.GetCarByInfo(car.Make, car.Model, car.Year)).Returns(car);

        // Act
        var result = _controller.Car(car.Make, car.Model, car.Year.ToString());

        // Assert
        var viewResult = result as ViewResult;

        viewResult.Should().NotBeNull();
        viewResult!.Model.Should().Be(car);
    }

    [Fact]
    public void Car_CarNotFound_ShouldReturnErrorView()
    {
        // Arrange
        _mockCarService.Setup(service => service.GetCarByInfo("Make", "Model", 2020)).Returns<Car?>(null);

        // Act
        var result = _controller.Car("Make", "Model", "2020");

        // Assert
        var viewResult = result as ViewResult;

        viewResult.Should().NotBeNull();
        viewResult!.ViewName.Should().Be("Error");
    }
}