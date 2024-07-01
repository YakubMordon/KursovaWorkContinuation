using FluentAssertions;
using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Domain.Entities;
using KursovaWork.Domain.Models;
using KursovaWork.UI.Controllers;
using KursovaWork.UI.Unit.Tests.Fakers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;

namespace KursovaWork.UI.Unit.Tests.Controllers;

public class ChangePasswordControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly ChangePasswordController _controller;

    private readonly UserFaker _userFaker;

    public ChangePasswordControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new ChangePasswordController(_mockUserService.Object);

        _userFaker = new UserFaker(); 
    }

    [Fact]
    public void UserFinder_ReturnsCorrectView()
    {
        // Act
        var result = _controller.UserFinder() as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ForgotPassword/UserFinder.cshtml");
    }

    [Fact]
    public void ForgotPassword_ValidEmail_ReturnsJsonWithSuccess()
    {
        // Arrange
        var user = _userFaker.Generate();
        var model = new EmailViewModel { Email = user.Email };

        _mockUserService.Setup(s => s.GetUserByEmail(model.Email)).Returns(user);

        // Act
        var result = _controller.ForgotPassword(model) as JsonResult;

        // Assert
        result.Should().NotBeNull();
        var jsonResult = JObject.FromObject(result.Value);

        var success = (bool)jsonResult["success"];

        success.Should().BeTrue();
    }

    [Fact]
    public void ForgotPassword_InvalidEmail_ReturnsJsonWithError()
    {
        // Arrange
        var model = new EmailViewModel { Email = "test@example.com" };

        _mockUserService.Setup(s => s.GetUserByEmail(model.Email)).Returns<User?>(null);

        // Act
        var result = _controller.ForgotPassword(model) as JsonResult;

        // Assert
        result.Should().NotBeNull();
        var jsonResult = JObject.FromObject(result.Value);

        var success = (bool)jsonResult["success"];
        var error = (string)jsonResult["error"];

        success.Should().BeFalse();
        error.Should().Be("Such email is not registered");
    }

    [Fact]
    public void ChangePassword_ValidCode_ReturnsJsonWithSuccess()
    {
        // Arrange
        var model = new VerificationViewModel { VerificationDigits = new[] { "1", "2", "3", "4" } };
        ChangePasswordController.VerificationCode = 1234;

        // Act
        var result = _controller.ChangePassword(model) as JsonResult;

        // Assert
        result.Should().NotBeNull();

        var jsonResult = JObject.FromObject(result.Value);

        var success = (bool)jsonResult["success"];

        success.Should().BeTrue();
    }

    [Fact]
    public void ChangePassword_InvalidCode_ReturnsJsonWithError()
    {
        // Arrange
        var model = new VerificationViewModel { VerificationDigits = new[] { "1", "2", "3", "5" } };
        ChangePasswordController.VerificationCode = 1234;

        // Act
        var result = _controller.ChangePassword(model) as JsonResult;

        // Assert
        result.Should().NotBeNull();
        var jsonResult = JObject.FromObject(result.Value);

        var success = (bool)jsonResult["success"];
        var error = (string)jsonResult["error"];

        success.Should().BeFalse();
        error.Should().Be("Incorrect verification code");
    }

    [Fact]
    public void ChangePassword_IncompleteDigits_ReturnsJsonWithError()
    {
        // Arrange
        var model = new VerificationViewModel { VerificationDigits = new[] { "1", "2", "3", "" } };

        // Act
        var result = _controller.ChangePassword(model) as JsonResult;

        // Assert
        result.Should().NotBeNull();
        var jsonResult = JObject.FromObject(result.Value);

        var success = (bool)jsonResult["success"];
        var error = (string)jsonResult["error"];

        success.Should().BeFalse();
        error.Should().Be("Not all digits entered");
    }

    [Fact]
    public void UpdatePassword_ReturnsCorrectView()
    {
        // Act
        var result = _controller.UpdatePassword() as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ForgotPassword/ChangePassword.cshtml");
    }

    [Fact]
    public void SubmitChange_ValidModel_ReturnsJsonWithSuccess()
    {
        // Arrange
        var user = _userFaker.Generate();
        var model = new ChangePasswordViewModel { Password = user.Password, ConfirmPassword = user.ConfirmPassword };

        ChangePasswordController.CurUser = user;

        // Act
        var result = _controller.SubmitChange(model) as JsonResult;

        // Assert
        result.Should().NotBeNull();

        var jsonResult = JObject.FromObject(result.Value);

        var success = (bool)jsonResult["success"];

        success.Should().BeTrue();
    }

    [Fact]
    public void SubmitChange_InvalidModel_ReturnsJsonWithErrors()
    {
        // Arrange
        var model = new ChangePasswordViewModel { Password = "newpass123", ConfirmPassword = "wrongpass" };
        _controller.ModelState.AddModelError(nameof(ChangePasswordViewModel.ConfirmPassword), "Passwords do not match");

        // Act
        var result = _controller.SubmitChange(model) as JsonResult;

        // Assert
        result.Should().NotBeNull();

        var jsonResult = JObject.FromObject(result.Value);

        var success = (bool)jsonResult["success"];
        var errors = JObject.FromObject(jsonResult["errors"]);

        var confirmPasswordError = (string)errors["confirmPasswordError"];

        success.Should().BeFalse();
        confirmPasswordError.Should().Be("Passwords do not match");
    }

    [Fact]
    public void SendVerificationCode_ReturnsCorrectView()
    {
        // Act
        var result = _controller.SendVerificationCode() as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ForgotPassword/ForgotPassword.cshtml");
    }

    [Fact]
    public void ReSendVerificationCode_ReturnsJsonWithSuccessMessage()
    {
        // Arrange
        var user = _userFaker.Generate();
        ChangePasswordController.CurUser = user;

        // Act
        var result = _controller.ReSendVerificationCode() as JsonResult;

        // Assert
        result.Should().NotBeNull();

        var jsonResult = JObject.FromObject(result.Value);

        var message = (string)jsonResult["message"];

        message.Should().Be("Verification code successfully sent");
    }

    [Fact]
    public void SendCode_SetsVerificationCode()
    {
        // Arrange
        var user = _userFaker.Generate();
        ChangePasswordController.CurUser = user;

        // Act
        _controller.SendCode();

        // Assert
        ChangePasswordController.VerificationCode.Should().BeInRange(1000, 9999);
    }
}