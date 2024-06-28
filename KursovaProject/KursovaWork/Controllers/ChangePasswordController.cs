using KursovaWorkDAL.Entity.Entities;
using KursovaWork.Models;
using KursovaWorkBLL.Services.AdditionalServices;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using KursovaWorkBLL.Contracts;

namespace KursovaWork.Controllers
{
    /// <summary>
    /// Controller responsible for handling user password change actions.
    /// </summary>
    public class ChangePasswordController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<ChangePasswordController> _logger;

        private static User? _curUser;
        private static int _verificationCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordController"/> class.
        /// </summary>
        /// <param name="userService">The service for user operations.</param>
        /// <param name="logger">The logger for logging.</param>
        public ChangePasswordController(IUserService userService, ILogger<ChangePasswordController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the page for entering user email.
        /// </summary>
        /// <returns>The page for entering user email.</returns>
        public IActionResult UserFinder()
        {
            _logger.LogInformation("Navigating to user email entry page");
            return View("~/Views/ForgotPassword/UserFinder.cshtml");
        }

        /// <summary>
        /// Processes the entered user email for sending a verification code.
        /// </summary>
        /// <param name="model">The model containing the entered user email.</param>
        /// <returns>The verification code entry page or the email entry page with an error message.</returns>
        public IActionResult ForgotPassword(EmailViewModel model)
        {
            _logger.LogInformation("Entering email verification method");
            if (ModelState.IsValid)
            {
                _curUser = _userService.GetUserByEmail(model.Email);

                if (_curUser is not null)
                {
                    _logger.LogInformation("Email found");
                    SendCode();
                    return Json(new { success = true });
                }

                _logger.LogInformation("Email not registered");
                return Json(new { success = false, error = "Such email is not registered" });
            }

            var error = ModelState[nameof(EmailViewModel.Email)].Errors.FirstOrDefault()?.ErrorMessage ?? "";

            _logger.LogInformation("Data did not pass validation");
            return Json(new { success = false, error });
        }

        /// <summary>
        /// Processes the entered verification code to navigate to the user password change page.
        /// </summary>
        /// <param name="model">The model containing the entered verification code.</param>
        /// <returns>The password change page or the verification code entry page with an error message.</returns>
        public IActionResult ChangePassword(VerificationViewModel model)
        {
            _logger.LogInformation("Entering verification code method");
            if (ModelState.IsValid)
            {
                var stringBuilder = new StringBuilder();
                _logger.LogInformation("Entering loop to construct a complete string of 4 substrings");

                foreach (var digit in model.VerificationDigits)
                {
                    if (string.IsNullOrEmpty(digit))
                    {
                        _logger.LogInformation("Not all digits entered");
                        return Json(new { success = false, error = "Not all digits entered" });
                    }

                    stringBuilder.Append(digit);
                }

                var temp = stringBuilder.ToString();

                if (int.Parse(temp) != _verificationCode)
                {
                    _logger.LogInformation("Incorrect verification code");
                    return Json(new { success = false, error = "Incorrect verification code" });
                }

                _logger.LogInformation("Successfully verified user's ability to change password");
                return Json(new { success = true });
            }

            var error = ModelState[nameof(VerificationViewModel.VerificationDigits)].Errors.FirstOrDefault()?.ErrorMessage ?? "";

            _logger.LogInformation("Data did not pass validation");
            return Json(new { success = false, error });
        }

        /// <summary>
        /// Navigates to the password change page.
        /// </summary>
        /// <returns>The password change page.</returns>
        public IActionResult UpdatePassword()
        {
            _logger.LogInformation("Navigating to the password change page");
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
            _logger.LogInformation("Entering password change method");

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Data passed validation");

                _userService.UpdatePasswordOfUser(model.Password, model.ConfirmPassword, _curUser);

                _logger.LogInformation("Password changed successfully, navigating to the main page");
                return Json(new { success = true });
            }

            var errors = new
            {
                passwordError = ModelState[nameof(ChangePasswordViewModel.Password)].Errors.FirstOrDefault()?.ErrorMessage ?? "",
                confirmPasswordError = ModelState[nameof(ChangePasswordViewModel.ConfirmPassword)].Errors.FirstOrDefault()?.ErrorMessage ?? ""
            };

            _logger.LogInformation("Data did not pass validation");
            return Json(new { success = false, errors });
        }

        /// <summary>
        /// Sends an email with a verification code.
        /// </summary>
        /// <returns>The verification code entry page.</returns>
        public IActionResult SendVerificationCode()
        {
            _logger.LogInformation("Navigating to the verification code entry page");

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
            _logger.LogInformation("Entering verification code sending method");

            _verificationCode = new Random().Next(1000, 9999);

            var subject = "Verification Code";

            var body = EmailBodyHelper.BodyTemp(_curUser.FirstName, _curUser.LastName, _verificationCode, "password change");

            _logger.LogInformation("Sending email message to user's email");

            EmailSender.SendEmail(_curUser.Email, subject, body);
        }
    }
}
