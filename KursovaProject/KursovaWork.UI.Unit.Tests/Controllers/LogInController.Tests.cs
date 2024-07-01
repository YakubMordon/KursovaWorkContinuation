using FluentAssertions;
using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Domain.Entities;
using KursovaWork.Domain.Models;
using KursovaWork.Infrastructure.Services.DB.Fakers;
using KursovaWork.UI.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Security.Claims;

namespace KursovaWork.UI.Unit.Tests.Controllers;

public class LogInControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly LogInController _controller;

    public LogInControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new LogInController(_mockUserService.Object);
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
    public void SignUp_ShouldReturnHomeView()
    {
        // Act
        var result = _controller.SignUp();

        // Assert
        var viewResult = result as ViewResult;

        viewResult.Should().NotBeNull();

        viewResult!.ViewName.Should().Be("~/Views/SignUp/SignUp.cshtml");
    }

    [Fact]
    public void LogIn_Post_ValidCredentials_ShouldReturnJsonSuccessTrue()
    {
        // Arrange
        var userFaker = new UserFaker();
        var user = userFaker.Generate();
        var validModel = new LogInViewModel { Email = user.Email, Password = user.Password };
        
        _mockUserService.Setup(s => s.ValidateUser(validModel.Email, validModel.Password)).Returns(user);

        var httpContext = new DefaultHttpContext();
        var authScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        // Mock the authentication services
        var authenticationService = new Mock<IAuthenticationService>();
        authenticationService
            .Setup(s => s.SignInAsync(httpContext,
                                      authScheme,
                                      It.IsAny<ClaimsPrincipal>(),
                                      It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);

        httpContext.RequestServices = new ServiceCollection()
            .AddAuthentication(options =>
            {
                options.DefaultScheme = authScheme;
                options.DefaultChallengeScheme = authScheme;
            })
            .AddCookie(authScheme, options => { })
            .Services
            .AddSingleton(authenticationService.Object)
            .BuildServiceProvider();

        var controllerContext = new ControllerContext
        {
            HttpContext = httpContext,
        };

        _controller.ControllerContext = controllerContext;

        // Act
        var result = _controller.LogIn(validModel);

        // Assert
        var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
        jsonResult.Value.Should().BeEquivalentTo(new { success = true });
    }

    [Fact]
    public void LogIn_Post_InvalidCredentials_ShouldReturnJsonWithErrorMessage()
    {
        // Arrange
        var invalidModel = new LogInViewModel { Email = "test@example.com", Password = "wrongpassword" };
        _mockUserService.Setup(s => s.ValidateUser(invalidModel.Email, invalidModel.Password)).Returns((User)null);

        // Act
        var result = _controller.LogIn(invalidModel);

        // Assert
        var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
        jsonResult.Value.Should().BeEquivalentTo(new { success = false, error = "Incorrect email or password." });
    }

    [Fact]
    public void LogIn_Post_InvalidModelState_ShouldReturnJsonWithValidationErrors()
    {
        // Arrange
        var invalidModel = new LogInViewModel { Email = "", Password = "" };
        _controller.ModelState.AddModelError(nameof(LogInViewModel.Email), "Email is required");
        _controller.ModelState.AddModelError(nameof(LogInViewModel.Password), "Password is required");

        // Act
        var result = _controller.LogIn(invalidModel);

        // Assert
        var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
        jsonResult.Value.Should().BeEquivalentTo(new
        {
            success = false,
            errors = new { emailError = "Email is required", passwordError = "Password is required" }
        });
    }
}
