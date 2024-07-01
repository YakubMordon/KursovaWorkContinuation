using Microsoft.AspNetCore.Mvc;
using System.Text;
using Serilog;
using KursovaWork.Domain.Entities;
using KursovaWork.Domain.Models;
using KursovaWork.Infrastructure.Services.Helpers.Static;
using KursovaWork.Application.Contracts.Services.Entities;

namespace KursovaWork.UI.Controllers;

/// <summary>
/// Controller responsible for handling user password change actions.
/// </summary>
public class ChangePasswordController : Controller
{
    private readonly IUserService _userService;
    public static User? CurUser;
    public static int VerificationCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangePasswordController"/> class.
    /// </summary>
    /// <param name="userService">The service for user operations.</param>
    public ChangePasswordController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Retrieves the page for entering user email.
    /// </summary>
    /// <returns>The page for entering user email.</returns>
    public IActionResult UserFinder()
    {
        Log.Information("Navigating to user email entry page");
        return View("~/Views/ForgotPassword/UserFinder.cshtml");
    }

    /// <summary>
    /// Processes the entered user email for sending a verification code.
    /// </summary>
    /// <param name="model">The model containing the entered user email.</param>
    /// <returns>The verification code entry page or the email entry page with an error message.</returns>
    public IActionResult ForgotPassword(EmailViewModel model)
    {
        Log.Information("Entering email verification method");
        if (ModelState.IsValid)
        {
            CurUser = _userService.GetUserByEmail(model.Email);

            if (CurUser is not null)
            {
                Log.Information("Email found");
                SendCode();
                return Json(new { success = true });
            }

            Log.Information("Email not registered");
            return Json(new { success = false, error = "Such email is not registered" });
        }

        var error = ModelState[nameof(EmailViewModel.Email)]?.Errors.FirstOrDefault()?.ErrorMessage ?? "";

        Log.Information("Data did not pass validation");
        return Json(new { success = false, error });
    }

    /// <summary>
    /// Processes the entered verification code to navigate to the user password change page.
    /// </summary>
    /// <param name="model">The model containing the entered verification code.</param>
    /// <returns>The password change page or the verification code entry page with an error message.</returns>
    public IActionResult ChangePassword(VerificationViewModel model)
    {
        Log.Information("Entering verification code method");
        if (ModelState.IsValid)
        {
            var stringBuilder = new StringBuilder();
            Log.Information("Entering loop to construct a complete string of 4 substrings");

            foreach (var digit in model.VerificationDigits)
            {
                if (string.IsNullOrEmpty(digit))
                {
                    Log.Information("Not all digits entered");
                    return Json(new { success = false, error = "Not all digits entered" });
                }

                stringBuilder.Append(digit);
            }

            var temp = stringBuilder.ToString();

            if (int.Parse(temp) != VerificationCode)
            {
                Log.Information("Incorrect verification code");
                return Json(new { success = false, error = "Incorrect verification code" });
            }

            Log.Information("Successfully verified user's ability to change password");
            return Json(new { success = true });
        }

        var error = ModelState[nameof(VerificationViewModel.VerificationDigits)]?.Errors.FirstOrDefault()?.ErrorMessage ?? "";

        Log.Information("Data did not pass validation");
        return Json(new { success = false, error });
    }

    /// <summary>
    /// Navigates to the password change page.
    /// </summary>
    /// <returns>The password change page.</returns>
    public IActionResult UpdatePassword()
    {
        Log.Information("Navigating to the password change page");
        return View("~/Views/ForgotPassword/ChangePassword.cshtml");
    }

    /// <summary>
    /// Processes the entered new password.
    /// </summary>
    /// <param name="model">The model containing the entered new password.</param>
    /// <returns>The password change page or the new password entry page with an error message.</returns>
    [HttpPost]
    public IActionResult SubmitChange(ChangePasswordViewModel model)
    {
        Log.Information("Entering password change method");

        if (ModelState.IsValid)
        {
            Log.Information("Data passed validation");

            _userService.UpdatePasswordOfUser(model.Password, model.ConfirmPassword, CurUser);

            Log.Information("Password changed successfully, navigating to the main page");
            return Json(new { success = true });
        }

        var errors = new
        {
            passwordError = ModelState[nameof(ChangePasswordViewModel.Password)]?.Errors.FirstOrDefault()?.ErrorMessage ?? "",
            confirmPasswordError = ModelState[nameof(ChangePasswordViewModel.ConfirmPassword)]?.Errors.FirstOrDefault()?.ErrorMessage ?? ""
        };

        Log.Information("Data did not pass validation");
        return Json(new { success = false, errors });
    }

    /// <summary>
    /// Sends an email with a verification code.
    /// </summary>
    /// <returns>The verification code entry page.</returns>
    public IActionResult SendVerificationCode()
    {
        Log.Information("Navigating to the verification code entry page");

        return View("~/Views/ForgotPassword/ForgotPassword.cshtml");
    }

    /// <summary>
    /// Resends an email with a new verification code.
    /// </summary>
    /// <returns>Success message of code resend.</returns>
    public IActionResult ReSendVerificationCode()
    {
        SendCode();

        return Json(new { message = "Verification code successfully sent" });
    }

    /// <summary>
    /// Method for generating and sending a verification code.
    /// </summary>
    public void SendCode()
    {
        Log.Information("Entering verification code sending method");

        VerificationCode = new Random().Next(1000, 9999);

        var subject = "Verification Code";

        var body = EmailBodyHelper.BodyTemp(CurUser.FirstName, CurUser.LastName, VerificationCode, "password change");

        Log.Information("Sending email message to user's email");

        EmailSenderHelper.SendEmail(CurUser.Email, subject, body);
    }
}