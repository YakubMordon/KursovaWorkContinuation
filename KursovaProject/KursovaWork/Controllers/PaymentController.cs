using KursovaWorkDAL.Entity.Entities.Car;
using KursovaWorkBLL.Services.AdditionalServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KursovaWorkBLL.Contracts;

namespace KursovaWork.Controllers
{
    /// <summary>
    /// Controller for handling payments.
    /// </summary>
    public class PaymentController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICardService _cardService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly ILogger<PaymentController> _logger;
        private static CarInfo _curCar;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController"/> class.
        /// </summary>
        /// <param name="carService">The service interface for performing car-related actions.</param>
        /// <param name="cardService">The service interface for performing card-related actions.</param>
        /// <param name="orderService">The service interface for performing order-related actions.</param>
        /// <param name="userService">The service interface for performing user-related actions.</param>
        /// <param name="logger">ILogger for logging events.</param>
        public PaymentController(ICarService carService, ICardService cardService, IOrderService orderService, IUserService userService, ILogger<PaymentController> logger)
        {
            _carService = carService;
            _cardService = cardService;
            _orderService = orderService;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Method for handling payment feasibility check.
        /// </summary>
        /// <param name="param1">Car make.</param>
        /// <param name="param2">Car model.</param>
        /// <param name="param3">Car year.</param>
        /// <returns>Operation result.</returns>
        public IActionResult Payment(string param1, string param2, string param3)
        {
            _logger.LogInformation("Entering method to check payment feasibility");

            _logger.LogInformation("Fetching user ID");
            var user = _userService.GetLoggedInUser();

            if (user is null)
            {
                _logger.LogInformation("User not logged in");
                return View("~/Views/Payment/NotLoggedIn.cshtml");
            }

            _logger.LogInformation("Fetching user payment method data");
            var creditCard = _cardService.GetByLoggedInUser();

            if (creditCard is null)
            {
                _logger.LogInformation("User has not added a payment method");
                return View("~/Views/Payment/CardNotConnected.cshtml");
            }

            var year = int.Parse(param3);

            var car = _carService.GetCarByInfo(param1, param2, year);

            if (car is not null)
            {
                _curCar = car;
                _logger.LogInformation("Model found successfully");

                _logger.LogInformation("Redirecting to payment confirmation");

                return View(car);
            }

            _logger.LogWarning("Model not found");
            return View("Error");
        }

        /// <summary>
        /// Method for handling successful payment.
        /// </summary>
        /// <returns>Operation result.</returns>
        public IActionResult Success()
        {
            _logger.LogInformation("Redirecting to method for confirming purchase payment");

            var id = _orderService.AddOrderLoggedIn(_curCar, ConfiguratorController.options);

            ConfiguratorController.options = null;

            _logger.LogInformation("Order number returned");

            _logger.LogInformation("Fetching user data");
            var user = _userService.GetLoggedInUser();

            var userName = user.FirstName + " " + user.LastName;
            var userEmail = user.Email;

            var subject = $"Car purchase #{id}";
            var body = EmailBodyHelper.OrderBodyTemp(userName, _curCar.Make, _curCar.Model, _curCar.Year);

            EmailSender.SendEmail(userEmail, subject, body);

            _logger.LogInformation("Order information sent to email");

            _logger.LogInformation("Redirecting to successful purchase execution page");

            return View("~/Views/Payment/Success.cshtml");
        }
    }
}
