using Microsoft.AspNetCore.Mvc;
using KursovaWork.Models;
using KursovaWorkDAL.Entity.Entities;
using KursovaWorkBLL.Services.AdditionalServices;
using System.Text;
using KursovaWorkBLL.Contracts;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace KursovaWork.Controllers
{
    /// <summary>
    /// Controller responsible for user registration.
    /// </summary>
    public class SignUpController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<SignUpController> _logger;
        private static User? _curUser;
        private static int _verificationCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpController"/> class.
        /// </summary>
        /// <param name="userService">The service interface for user-related actions.</param>
        /// <param name="logger">ILogger for logging events.</param>
        public SignUpController(IUserService userService, ILogger<SignUpController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the registration page.
        /// </summary>
        /// <returns>The registration page.</returns>
        public IActionResult SignUp()
        {
            _logger.LogInformation("Navigating to the registration page");
            return View();
        }

        /// <summary>
        /// Retrieves the login page.
        /// </summary>
        /// <returns>The login page.</returns>
        public IActionResult LogIn()
        {
            _logger.LogInformation("Navigating to the login page");
            return View("~/Views/LogIn/LogIn.cshtml");
        }

        /// <summary>
        /// Handles user registration data input.
        /// </summary>
        /// <param name="model">The model containing user registration data.</param>
        /// <returns>The verification code entry page or the registration page with an error message.</returns>
        [HttpPost]
        public IActionResult SignUp(SignUpViewModel model)
        {
            _logger.LogInformation("Entering user registration data verification method");

            var errors = new Dictionary<string, string>();

            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByEmail(model.Email);

                if (user != null)
                {
                    errors["emailError"] = "User with this email already exists.";
                    ModelState.AddModelError("Email", "User with this email already exists.");
                    _logger.LogInformation("User with this email already exists");
                    return Json(new { success = false, errors });
                }

                _curUser = model.ToUser();

                _logger.LogInformation("Successfully checked if email already exists");

                SendCode();

                return Json(new { success = true });
            }

            errors["firstNameError"] = ModelState[nameof(SignUpViewModel.FirstName)].Errors.FirstOrDefault()?.ErrorMessage ?? "";
            errors["lastNameError"] = ModelState[nameof(SignUpViewModel.LastName)].Errors.FirstOrDefault()?.ErrorMessage ?? "";
            errors["emailError"] = ModelState[nameof(SignUpViewModel.Email)].Errors.FirstOrDefault()?.ErrorMessage ?? "";
            errors["passwordError"] = ModelState[nameof(SignUpViewModel.Password)].Errors.FirstOrDefault()?.ErrorMessage ?? "";
            errors["confirmPasswordError"] = ModelState[nameof(SignUpViewModel.ConfirmPassword)].Errors.FirstOrDefault()?.ErrorMessage ?? "";

            _logger.LogInformation("Data did not pass validation");
            return Json(new { success = false, errors });
        }

        /// <summary>
        /// Handles user-entered verification code and confirms registration.
        /// </summary>
        /// <param name="verification">The model containing user-entered verification code.</param>
        /// <returns>The congratulations page or the verification code entry page with an error message.</returns>
        [HttpPost]
        public IActionResult Submit(VerificationViewModel verification)
        {
            _logger.LogInformation("Entering method to confirm user registration and validate verification code");

            if (ModelState.IsValid)
            {
                var stringBuilder = new StringBuilder();
                _logger.LogInformation("Entering loop to form a concatenated string of 4 substrings");

                foreach (var digit in verification.VerificationDigits)
                {
                    if (string.IsNullOrEmpty(digit))
                    {
                        _logger.LogInformation("Not all digits are entered");
                        return Json(new { success = false, error = "Not all digits are entered" });
                    }
                    stringBuilder.Append(digit);
                }

                var temp = stringBuilder.ToString();

                if (int.Parse(temp) != _verificationCode)
                {
                    _logger.LogInformation("Incorrect verification code");

                    return Json(new { success = false, error = "Incorrect verification code" });
                }

                _userService.AddUser(_curUser);
                _curUser = null;
                _logger.LogInformation("User successfully registered, redirecting to the main page");

                return Json(new { success = true });
            }

            var error = ModelState[nameof(VerificationViewModel.VerificationDigits)].Errors.FirstOrDefault()?.ErrorMessage ?? "";

            _logger.LogInformation("Data did not pass validation");
            return Json(new { success = false, error });
        }

        /// <summary>
        /// Redirects to the congratulations page.
        /// </summary>
        /// <returns>The congratulations page.</returns>
        [HttpGet]
        public IActionResult Congratulations()
        {
            return View("~/Views/SignUp/Congratulations.cshtml");
        }

        /// <summary>
        /// Sends an email with the verification code.
        /// </summary>
        /// <returns>The verification code entry page.</returns>
        public IActionResult SendVerificationCode()
        {
            _logger.LogInformation("Navigating to the code entry page");

            return View("~/Views/SignUp/Submit.cshtml");
        }

        /// <summary>
        /// Resends the email with a new verification code.
        /// </summary>
        /// <returns>Success message indicating code sent successfully.</returns>
        public IActionResult ReSendVerificationCode()
        {
            SendCode();
            return Json(new { message = "Code successfully sent" });
        }

        /// <summary>
        /// Generates and sends the verification code via email.
        /// </summary>
        public void SendCode()
        {
            _logger.LogInformation("Entering method to send verification code");

            _verificationCode = new Random().Next(1000, 9999);

            var subject = "Verification Code";
            var body = EmailBodyHelper.BodyTemp(_curUser.FirstName, _curUser.LastName, _verificationCode, "registration");

            _logger.LogInformation("Sending email notification to user's email");

            EmailSender.SendEmail(_curUser.Email, subject, body);
        }
    }
}
