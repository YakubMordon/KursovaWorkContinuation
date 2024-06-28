using KursovaWork.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using KursovaWorkBLL.Contracts;

namespace KursovaWork.Controllers
{
    /// <summary>
    /// Controller responsible for user login operations.
    /// </summary>
    public class LogInController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<LogInController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogInController"/> class.
        /// </summary>
        /// <param name="userService">The service interface for handling users.</param>
        /// <param name="logger">ILogger for logging events.</param>
        public LogInController(IUserService userService, ILogger<LogInController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the login page.
        /// </summary>
        /// <returns>The login page.</returns>
        public IActionResult LogIn()
        {
            _logger.LogInformation("Redirecting to the login page");
            return View();
        }

        /// <summary>
        /// Retrieves the sign-up page.
        /// </summary>
        /// <returns>The sign-up page.</returns>
        public IActionResult SignUp()
        {
            _logger.LogInformation("Redirecting to the sign-up page");
            return View("~/Views/SignUp/SignUp.cshtml");
        }

        /// <summary>
        /// Handles user login data input and authenticates the user.
        /// </summary>
        /// <param name="model">The model containing user login input data.</param>
        /// <returns>Main menu page or login page with error message.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogIn(LogInViewModel model)
        {
            _logger.LogInformation("Entering method to check login data");
            if (ModelState.IsValid)
            {
                var user = _userService.ValidateUser(model.Email, model.Password);

                if (user is not null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties).Wait();

                    _logger.LogInformation("User successfully logged into the account");

                    return Json(new { success = true });
                }

                _logger.LogInformation("Incorrect email or password entered");
                var error = "Incorrect email or password.";

                return Json(new { success = false, error });
            }

            var errors = new
            {
                emailError = ModelState[nameof(LogInViewModel.Email)].Errors.FirstOrDefault()?.ErrorMessage ?? "",
                passwordError = ModelState[nameof(LogInViewModel.Password)].Errors.FirstOrDefault()?.ErrorMessage ?? "",
            };

            _logger.LogInformation("Data did not pass validation");
            return Json(new { success = false, errors });
        }
    }
}
