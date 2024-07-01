using FluentAssertions;
using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Domain.Models;
using KursovaWork.UI.Controllers;
using KursovaWork.UI.Unit.Tests.Fakers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace KursovaWork.UI.Unit.Tests.Controllers;

public class HomeControllerTests
{
    private readonly Mock<IOrderService> _mockOrderService;
    private readonly HomeController _controller;
    private readonly OrderFaker _orderFaker;

    public HomeControllerTests()
    {
        _mockOrderService = new Mock<IOrderService>();
        _controller = new HomeController(_mockOrderService.Object);

        _orderFaker = new OrderFaker();
    }

    [Fact]
    public void Index_ShouldReturnHomeView()
    {
        // Act
        var result = _controller.Index();

        // Assert
        var viewResult = result as ViewResult;

        viewResult.Should().NotBeNull();

        viewResult!.ViewName.Should().Be("~/Views/Home/Index.cshtml");
    }

    [Fact]
    public void LogIn_ShouldReturnLogInView()
    {
        // Act
        var result = _controller.LogIn();

        // Assert
        var viewResult = result as ViewResult;

        viewResult.Should().NotBeNull();

        viewResult!.ViewName.Should().Be("~/Views/LogIn/LogIn.cshtml");
    }

    [Fact]
    public void LogOut_ShouldSignOutAndRedirectToHome()
    {
        // Arrange
        var mockAuth = new Mock<IAuthenticationService>();
        var mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
        var mockUrlHelper = new Mock<IUrlHelper>();

        mockUrlHelperFactory
            .Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>()))
            .Returns(mockUrlHelper.Object);

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                RequestServices = new ServiceCollection()
                    .AddSingleton(mockAuth.Object)
                    .AddSingleton(mockUrlHelperFactory.Object)
                    .BuildServiceProvider()
            }
        };

        // Act
        var result = _controller.LogOut();

        // Assert
        var redirectResult = result as RedirectToActionResult;

        redirectResult.Should().NotBeNull();
        redirectResult!.ActionName.Should().Be("Index");
        redirectResult.ControllerName.Should().Be("Home");

        mockAuth.Verify(a => a.SignOutAsync(It.IsAny<HttpContext>(), CookieAuthenticationDefaults.AuthenticationScheme, null), Times.Once);
    }

    [Fact]
    public void ModelList_ShouldClearOptionsAndRedirectToModelList()
    {
        // Act
        var result = _controller.ModelList();

        // Assert
        ConfiguratorController.Options.Should().BeNull();
        var redirectResult = result as RedirectToActionResult;
        redirectResult.Should().NotBeNull();
        redirectResult!.ActionName.Should().Be("ModelList");
        redirectResult.ControllerName.Should().Be("ModelList");
    }

    [Fact]
    public void OrderList_ShouldReturnOrderListViewWithOrders()
    {
        // Arrange
        var orders = _orderFaker.Generate(5);

        _mockOrderService.Setup(service => service.FindAllLoggedIn()).Returns(orders);

        // Act
        var result = _controller.OrderList();

        // Assert
        var viewResult = result as ViewResult;

        viewResult.Should().NotBeNull();

        viewResult!.ViewName.Should().Be("~/Views/OrderList/OrderList.cshtml");

        viewResult.Model.Should().BeEquivalentTo(orders);
    }

    [Fact]
    public void Error_ShouldReturnErrorView()
    {
        // Arrange
        var traceIdentifier = "test-trace-identifier";

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
        };

        _controller.ControllerContext.HttpContext.TraceIdentifier = traceIdentifier;

        // Act
        var result = _controller.Error();

        // Assert
        var viewResult = result as ViewResult;

        viewResult.Should().NotBeNull();
        viewResult!.ViewName.Should().Be(null);
        viewResult.Model.Should().BeOfType<ErrorViewModel>();

        var model = viewResult.Model as ErrorViewModel;
        model.Should().NotBeNull();
        model!.RequestId.Should().Be(traceIdentifier);
    }
}