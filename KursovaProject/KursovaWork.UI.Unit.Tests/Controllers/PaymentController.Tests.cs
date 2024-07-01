using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Domain.Entities.Car;
using KursovaWork.Domain.Entities;
using KursovaWork.UI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using KursovaWork.UI.Unit.Tests.Fakers;

namespace KursovaWork.UI.Unit.Tests.Controllers;

public class PaymentControllerTests
{
    private readonly Mock<ICarService> _mockCarService;
    private readonly Mock<ICardService> _mockCardService;
    private readonly Mock<IOrderService> _mockOrderService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly PaymentController _controller;

    private readonly CarFaker _carFaker;

    public PaymentControllerTests()
    {
        _mockCarService = new Mock<ICarService>();
        _mockCardService = new Mock<ICardService>();
        _mockOrderService = new Mock<IOrderService>();
        _mockUserService = new Mock<IUserService>();
        _controller = new PaymentController(_mockCarService.Object, _mockCardService.Object, _mockOrderService.Object, _mockUserService.Object);

        _carFaker = new CarFaker();
    }

    [Fact]
    public void Payment_UserNotLoggedIn_ReturnsNotLoggedInView()
    {
        // Arrange
        var car = _carFaker.Generate();

        _mockUserService.Setup(s => s.GetLoggedInUser()).Returns((User)null);

        // Act
        var result = _controller.Payment(car.Make, car.Model, car.Year.ToString()) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/Payment/NotLoggedIn.cshtml");
    }

    [Fact]
    public void Payment_UserWithoutCard_ReturnsCardNotConnectedView()
    {
        // Arrange
        var car = _carFaker.Generate();

        _mockUserService.Setup(s => s.GetLoggedInUser()).Returns(new User());

        _mockCardService.Setup(s => s.GetByLoggedInUser()).Returns<Card?>(null);

        // Act
        var result = _controller.Payment(car.Make, car.Model, car.Year.ToString()) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/Payment/CardNotConnected.cshtml");
    }

    [Fact]
    public void Payment_CarNotFound_ReturnsErrorView()
    {
        // Arrange
        var car = _carFaker.Generate();

        _mockUserService.Setup(s => s.GetLoggedInUser()).Returns(new User());

        _mockCardService.Setup(s => s.GetByLoggedInUser()).Returns(new Card());

        _mockCarService.Setup(s => s.GetCarByInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .Returns<Car?>(null);

        // Act
        var result = _controller.Payment(car.Make, car.Model, car.Year.ToString()) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("Error");
    }

    [Fact]
    public void Payment_CarFound_ReturnsPaymentView()
    {
        // Arrange
        var car = _carFaker.Generate();

        _mockUserService.Setup(s => s.GetLoggedInUser()).Returns(new User());
        _mockCardService.Setup(s => s.GetByLoggedInUser()).Returns(new Card());
        _mockCarService.Setup(s => s.GetCarByInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(car);

        // Act
        var result = _controller.Payment(car.Make, car.Model, car.Year.ToString()) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/Payment/Payment.cshtml");
        result.Model.Should().Be(car);
    }

    [Fact]
    public void Success_OrderCreated_ReturnsSuccessView()
    {
        // Arrange
        var userFaker = new UserFaker();

        var car = _carFaker.Generate();
        var user = userFaker.Generate();

        _mockUserService.Setup(s => s.GetLoggedInUser()).Returns(user);
        _mockCardService.Setup(s => s.GetByLoggedInUser()).Returns(new Card());
        _mockCarService.Setup(s => s.GetCarByInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(car);
        _mockOrderService.Setup(s => s.AddOrderLoggedIn(It.IsAny<Car>(), It.IsAny<ConfiguratorOptions?>())).Returns(1);

        _controller.Payment(car.Make, car.Model, car.Year.ToString());

        // Act
        var result = _controller.Success() as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/Payment/Success.cshtml");
    }
}