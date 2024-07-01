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

public class CreditCardControllerTests
{
    private readonly Mock<ICardService> _mockCardService;
    private readonly CreditCardController _controller;

    private readonly CardFaker _cardFaker;

    public CreditCardControllerTests()
    {
        _mockCardService = new Mock<ICardService>();
        _controller = new CreditCardController(_mockCardService.Object);

        _cardFaker = new CardFaker();
    }

    [Fact]
    public void CreditCard_WhenCardExists_ShouldReturnViewWithCardInfo()
    {
        // Arrange
        var card = _cardFaker.Generate();
        _mockCardService.Setup(cs => cs.CardExists()).Returns(true);
        _mockCardService.Setup(cs => cs.GetByLoggedInUser()).Returns(card);

        // Act
        var result = _controller.CreditCard();

        // Assert
        var viewResult = result as ViewResult;
        viewResult.Should().NotBeNull();

        viewResult!.ViewName.Should().Be("~/Views/CreditCard/CreditCard.cshtml");

        viewResult.ViewData.ContainsKey("CardNumber").Should().BeTrue();
        viewResult.ViewData.ContainsKey("CardHolderName").Should().BeTrue();
        viewResult.ViewData.ContainsKey("Month").Should().BeTrue();
        viewResult.ViewData.ContainsKey("Year").Should().BeTrue();

        var cardLastDigits = card.CardNumber.Substring(card.CardNumber.Length - 4);

        var actualCardNumber = _controller.ViewBag.CardNumber as string;
        var actualCardHolderName = _controller.ViewBag.CardHolderName as string;
        var actualMonth = _controller.ViewBag.Month as string;
        var actualYear = _controller.ViewBag.Year as string;
        var isCardExisting = (bool)_controller.ViewBag.Card;

        actualCardNumber.Should().Be($"···· ···· ···· {cardLastDigits}");
        actualCardHolderName.Should().Be(card.CardHolderName);
        actualMonth.Should().Be(card.ExpirationMonth);
        actualYear.Should().Be(card.ExpirationYear);
        isCardExisting.Should().BeTrue();
    }

    [Fact]
    public void CreditCard_WhenNoCardExists_ShouldReturnViewWithoutCardInfo()
    {
        // Arrange
        _mockCardService.Setup(cs => cs.CardExists()).Returns(false);

        // Act
        var result = _controller.CreditCard();

        // Assert
        var viewResult = result as ViewResult;
        viewResult.Should().NotBeNull();

        viewResult!.ViewName.Should().Be("~/Views/CreditCard/CreditCard.cshtml");

        viewResult.ViewData.ContainsKey("CardNumber").Should().BeFalse();
        viewResult.ViewData.ContainsKey("CardHolderName").Should().BeFalse();
        viewResult.ViewData.ContainsKey("Month").Should().BeFalse();
        viewResult.ViewData.ContainsKey("Year").Should().BeFalse();
    }

    [Fact]
    public void SetCreditCardInfo_ShouldSetViewBagWithCardInfo()
    {
        // Arrange
        var card = _cardFaker.Generate();

        _mockCardService.Setup(cs => cs.GetByLoggedInUser()).Returns(card);

        // Act
        _controller.SetCreditCardInfo();

        // Assert
        var cardLastDigits = card.CardNumber.Substring(card.CardNumber.Length - 4);

        var actualCardNumber = _controller.ViewBag.CardNumber as string;
        var actualCardHolderName = _controller.ViewBag.CardHolderName as string;
        var actualMonth = _controller.ViewBag.Month as string;
        var actualYear = _controller.ViewBag.Year as string;
        var isCardExisting = (bool)_controller.ViewBag.Card;

        actualCardNumber.Should().Be($"···· ···· ···· {cardLastDigits}");
        actualCardHolderName.Should().Be(card.CardHolderName);
        actualMonth.Should().Be(card.ExpirationMonth);
        actualYear.Should().Be(card.ExpirationYear);
        isCardExisting.Should().BeTrue();
    }

    [Fact]
    public void CreditCard_Post_ValidModel_ShouldReturnJsonSuccessTrue()
    {
        // Arrange
        var entity = _cardFaker.Generate();

        var model = new CreditCardViewModel
        {
            CardHolderName = entity.CardHolderName,
            CardNumber = entity.CardNumber,
            ExpirationMonth = entity.ExpirationMonth,
            ExpirationYear = entity.ExpirationYear,
            Cvv = entity.Cvv
        };

        _mockCardService.Setup(cs => cs.AddCard(It.IsAny<Card>()));

        // Act
        var result = _controller.CreditCard(model);

        // Assert
        var jsonResult = result as JsonResult;
        jsonResult.Should().NotBeNull();

        var jsonData = JObject.FromObject(jsonResult!.Value);
        jsonData["success"].Should().NotBeNull();

        var success = (bool)jsonData["success"];
        success.Should().BeTrue();
    }

    [Fact]
    public void CreditCard_Post_InvalidModel_ShouldReturnJsonWithErrors()
    {
        // Arrange
        var invalidModel = new CreditCardViewModel();

        // Simulate ModelState error for CardHolderName
        _controller.ModelState.AddModelError(nameof(CreditCardViewModel.CardHolderName), "Card holder name is required");

        // Act
        var result = _controller.CreditCard(invalidModel);

        // Assert
        var jsonResult = result as JsonResult;
        jsonResult.Should().NotBeNull();

        var jsonData = JObject.FromObject(jsonResult!.Value);
        jsonData["success"].Should().NotBeNull();

        var success = (bool)jsonData["success"];
        success.Should().BeFalse();

        var errors = JObject.FromObject(jsonData["errors"]);
        errors.Should().NotBeNull();
        errors.Should().ContainKey("CardHolderName");

        var actualHolderName = (string)errors["CardHolderName"];
        actualHolderName.Should().Be("Card holder name is required");
    }

    [Fact]
    public void DeleteCreditCard_ShouldDeleteCardAndReturnRedirect()
    {
        // Arrange
        _mockCardService.Setup(cs => cs.DeleteCard());

        // Act
        var result = _controller.DeleteCreditCard();

        // Assert
        var viewResult = result as ViewResult;
        viewResult.Should().NotBeNull();
        viewResult!.ViewName.Should().Be("~/Views/CreditCard/CreditCard.cshtml");
    }
}