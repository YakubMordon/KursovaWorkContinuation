using Microsoft.AspNetCore.Mvc;
using Serilog;
using KursovaWork.Domain.Models;
using KursovaWork.Infrastructure.Mappers.Entities;
using KursovaWork.Application.Contracts.Services.Entities;

namespace KursovaWork.UI.Controllers;

/// <summary>
/// Controller responsible for payment method operations.
/// </summary>
public class CreditCardController : Controller
{
    private readonly ICardService _cardService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreditCardController"/> class.
    /// </summary>
    /// <param name="cardService">The service interface for handling requests.</param>
    public CreditCardController(ICardService cardService)
    {
        _cardService = cardService;
    }

    /// <summary>
    /// Transition to the payment methods page.
    /// </summary>
    /// <returns>The payment methods page.</returns>
    public IActionResult CreditCard()
    {
        Log.Information("Transitioning to payment methods page method");

        Log.Information("Fields for adding a card are excluded during loading");
        ViewBag.Input = false;

        if (_cardService.CardExists())
        {
            Log.Information("Payment method is connected to the user");

            SetCreditCardInfo();
        }

        Log.Information("Transitioning to payment methods page");
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

        Log.Information("Retrieving all user payment method data");
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
        Log.Information("Entering payment method removal method");

        if (ModelState.IsValid)
        {
            var mapper = new CardMapper();

            var entity = mapper.ModelToEntity(model);

            _cardService.AddCard(entity);

            return Json(new { success = true });
        }

        Log.Information("Data did not pass verification");

        var errors = new
        {
            CardHolderName = ModelState[nameof(CreditCardViewModel.CardHolderName)]?.Errors.FirstOrDefault()?.ErrorMessage ?? "",
            CardNumber = ModelState[nameof(CreditCardViewModel.CardNumber)]?.Errors.FirstOrDefault()?.ErrorMessage ?? "",
            ExpirationMonth = ModelState[nameof(CreditCardViewModel.ExpirationMonth)]?.Errors.FirstOrDefault()?.ErrorMessage ?? "",
            ExpirationYear = ModelState[nameof(CreditCardViewModel.ExpirationYear)]?.Errors.FirstOrDefault()?.ErrorMessage ?? "",
            CVV = ModelState[nameof(CreditCardViewModel.Cvv)]?.Errors.FirstOrDefault()?.ErrorMessage ?? ""
        };

        Log.Information("Displaying input fields immediately after loading");
        ViewBag.Input = true;
        return Json(new { success = false, errors });
    }

    /// <summary>
    /// Deletes the payment method.
    /// </summary>
    /// <returns>Redirects to the payment methods page.</returns>
    public IActionResult DeleteCreditCard()
    {
        Log.Information("Entering payment method removal method");

        _cardService.DeleteCard();

        Log.Information("Fields for adding a card are excluded during loading");
        ViewBag.Input = false;

        Log.Information("Transitioning to payment methods page");
        return View("~/Views/CreditCard/CreditCard.cshtml");
    }
}