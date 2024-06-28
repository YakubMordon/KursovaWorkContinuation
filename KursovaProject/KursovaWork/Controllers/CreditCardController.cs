using KursovaWork.Models;
using Microsoft.AspNetCore.Mvc;
using KursovaWorkBLL.Contracts;

namespace KursovaWork.Controllers
{
    /// <summary>
    /// Controller responsible for payment method operations.
    /// </summary>
    public class CreditCardController : Controller
    {
        private readonly ICardService _cardService;
        private readonly ILogger<CreditCardController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCardController"/> class.
        /// </summary>
        /// <param name="cardService">The service interface for handling requests.</param>
        /// <param name="logger">ILogger object for logging events.</param>
        public CreditCardController(ICardService cardService, ILogger<CreditCardController> logger)
        {
            _cardService = cardService;
            _logger = logger;
        }

        /// <summary>
        /// Transition to the payment methods page.
        /// </summary>
        /// <returns>The payment methods page.</returns>
        public IActionResult CreditCard()
        {
            _logger.LogInformation("Transitioning to payment methods page method");

            _logger.LogInformation("Fields for adding a card are excluded during loading");
            ViewBag.Input = false;

            if (_cardService.CardExists())
            {
                _logger.LogInformation("Payment method is connected to the user");

                SetCreditCardInfo();
            }

            _logger.LogInformation("Transitioning to payment methods page");
            return View("~/Views/CreditCard/CreditCard.cshtml");
        }

        /// <summary>
        /// Records credit card information into ViewBag.
        /// </summary>
        public void SetCreditCardInfo()
        {
            var user = _cardService.GetByLoggedInUser();
            var cardNumber = user.CardNumber;

            ViewBag.CardNumber = "···· ···· ···· " + cardNumber.Substring(cardNumber.Length - 4);
            ViewBag.CardHolderName = user.CardHolderName;
            ViewBag.Month = user.ExpirationMonth;
            ViewBag.Year = user.ExpirationYear;
            ViewBag.Card = true;

            _logger.LogInformation("Retrieving all user payment method data");
        }

        /// <summary>
        /// Processes the payment method addition form.
        /// </summary>
        /// <param name="model">The payment method data model.</param>
        /// <returns>Redirects to the main page or returns the form again with errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreditCard(CreditCardViewModel model)
        {
            _logger.LogInformation("Entering payment method removal method");

            if (ModelState.IsValid)
            {
                _cardService.AddCard(model.ToCard());

                return Json(new { success = true });
            }

            _logger.LogInformation("Data did not pass verification");

            var errors = new
            {
                CardHolderName = ModelState[nameof(CreditCardViewModel.CardHolderName)].Errors.FirstOrDefault()?.ErrorMessage ?? "",
                CardNumber = ModelState[nameof(CreditCardViewModel.CardNumber)].Errors.FirstOrDefault()?.ErrorMessage ?? "",
                ExpirationMonth = ModelState[nameof(CreditCardViewModel.ExpirationMonth)].Errors.FirstOrDefault()?.ErrorMessage ?? "",
                ExpirationYear = ModelState[nameof(CreditCardViewModel.ExpirationYear)].Errors.FirstOrDefault()?.ErrorMessage ?? "",
                CVV = ModelState[nameof(CreditCardViewModel.CVV)].Errors.FirstOrDefault()?.ErrorMessage ?? ""
            };

            _logger.LogInformation("Displaying input fields immediately after loading");
            ViewBag.Input = true;
            return Json(new { success = false, errors });
        }

        /// <summary>
        /// Deletes the payment method.
        /// </summary>
        /// <returns>Redirects to the payment methods page.</returns>
        public IActionResult DeleteCreditCard()
        {
            _logger.LogInformation("Entering payment method removal method");

            _cardService.DeleteCard();

            _logger.LogInformation("Fields for adding a card are excluded during loading");
            ViewBag.Input = false;

            _logger.LogInformation("Transitioning to payment methods page");
            return View("~/Views/CreditCard/CreditCard.cshtml");
        }
    }
}
