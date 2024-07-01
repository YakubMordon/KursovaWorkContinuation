using FluentAssertions;
using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Domain.Models;
using KursovaWork.UI.Controllers;
using KursovaWork.UI.Unit.Tests.Fakers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;

namespace KursovaWork.UI.Unit.Tests.Controllers;

public class SignUpControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly SignUpController _controller;

    public SignUpControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new SignUpController(_mockUserService.Object);
    }

    [Fact]
    public void SignUp_ReturnsCorrectView()
    {
        // Act
        var result = _controller.SignUp() as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/SignUp/SignUp.cshtml");
    }

    [Fact]
    public void LogIn_ReturnsCorrectView()
    {
        // Act
        var result = _controller.LogIn() as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/LogIn/LogIn.cshtml");
    }

    [Fact]
    public void SignUp_InvalidModelState_ReturnsJsonWithErrors()
    {
        // Arrange
        _controller.ModelState.AddModelError("Email", "Required");
        var model = new SignUpViewModel();

        // Act
        var result = _controller.SignUp(model) as JsonResult;

        // Assert
        result.Should().NotBeNull();
        var jsonResult = JObject.FromObject(result.Value);

        var success = (bool)jsonResult["success"];
        var errors = JObject.FromObject(jsonResult["errors"]);

        var emailError = (string) errors["emailError"];

        success.Should().BeFalse();
        emailError.Should().Be("Required");
    }

    [Fact]
    public void Submit_ValidVerificationCode_ReturnsJsonWithSuccess()
    {
        // Arrange
        var verificationCode = 1234;
        SignUpController.VerificationCode = verificationCode;
        var verification = new VerificationViewModel
        {
            VerificationDigits = new[] { "1", "2", "3", "4" }
        };

        // Act
        var result = _controller.Submit(verification) as JsonResult;

        // Assert
        result.Should().NotBeNull();
        var jsonResult = JObject.FromObject(result.Value);

        var success = (bool)jsonResult["success"];

        success.Should().BeTrue();
    }

    [Fact]
    public void Submit_InvalidVerificationCode_ReturnsJsonWithError()
    {
        // Arrange
        var verificationCode = 1234;
        SignUpController.VerificationCode = verificationCode;
        var verification = new VerificationViewModel
        {
            VerificationDigits = new[] { "1", "2", "3", "5" }
        };

        // Act
        var result = _controller.Submit(verification) as JsonResult;

        // Assert
        result.Should().NotBeNull();
        var jsonResult = JObject.FromObject(result.Value);

        var success = (bool)jsonResult["success"];
        var error = (string)jsonResult["error"];

        success.Should().BeFalse();
        error.Should().Be("Incorrect verification code");
    }

    [Fact]
    public void ReSendVerificationCode_SendsNewCode_ReturnsJsonWithMessage()
    {
        // Arrange
        var userFaker = new UserFaker();

        var user = userFaker.Generate();
        var verificationCode = 1234;
        SignUpController.VerificationCode = verificationCode;

        SignUpController.CurUser = user;

        // Act
        var result = _controller.ReSendVerificationCode() as JsonResult;

        // Assert
        result.Should().NotBeNull();
        var jsonResult = JObject.FromObject(result.Value);

        var message = (string)jsonResult["message"];

        message.Should().Be("Code successfully sent");
    }
}