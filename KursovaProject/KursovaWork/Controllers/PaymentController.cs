using KursovaWorkDAL.Entity.Entities.Car;
using Microsoft.AspNetCore.Mvc;
using KursovaWorkBLL.Contracts;
using Serilog;
using KursovaWorkBLL.Services.Helpers.Static;

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
        private static CarInfo _curCar;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController"/> class.
        /// </summary>
        /// <param name="carService">The service interface for performing car-related actions.</param>
        /// <param name="cardService">The service interface for performing card-related actions.</param>
        /// <param name="orderService">The service interface for performing order-related actions.</param>
        /// <param name="userService">The service interface for performing user-related actions.</param>
        public PaymentController(ICarService carService, ICardService cardService, IOrderService orderService, IUserService userService)
        {
            _carService = carService;
            _cardService = cardService;
            _orderService = orderService;
            _userService = userService;
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
            Log.Information("Entering method to check payment feasibility");

            Log.Information("Fetching user ID");
            var user = _userService.GetLoggedInUser();

            if (user is null)
            {
                Log.Information("User not logged in");
                return View("~/Views/Payment/NotLoggedIn.cshtml");
            }

            Log.Information("Fetching user payment method data");
            var creditCard = _cardService.GetByLoggedInUser();

            if (creditCard is null)
            {
                Log.Information("User has not added a payment method");
                return View("~/Views/Payment/CardNotConnected.cshtml");
            }

            var year = int.Parse(param3);

            var car = _carService.GetCarByInfo(param1, param2, year);

            if (car is not null)
            {
                _curCar = car;
                Log.Information("Model found successfully");

                Log.Information("Redirecting to payment confirmation");

                return View(car);
            }

            Log.Warning("Model not found");
            return View("Error");
        }

        /// <summary>
        /// Method for handling successful payment.
        /// </summary>
        /// <returns>Operation result.</returns>
        public IActionResult Success()
        {
            Log.Information("Redirecting to method for confirming purchase payment");

            var id = _orderService.AddOrderLoggedIn(_curCar, ConfiguratorController.Options);

            ConfiguratorController.Options = null;

            Log.Information("Order number returned");

            Log.Information("Fetching user data");
            var user = _userService.GetLoggedInUser();

            var userName = user.FirstName + " " + user.LastName;
            var userEmail = user.Email;

            var subject = $"Car purchase #{id}";
            var body = EmailBodyHelper.OrderBodyTemp(userName, _curCar.Make, _curCar.Model, _curCar.Year);

            EmailSenderHelper.SendEmail(userEmail, subject, body);

            Log.Information("Order information sent to email");

            Log.Information("Redirecting to successful purchase execution page");

            return View("~/Views/Payment/Success.cshtml");
        }
    }
}
