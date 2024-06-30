using FluentAssertions;
using KursovaWork.Application.Contracts.Repositories;
using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Application.Contracts.Services.Helpers.Transient;
using KursovaWork.Domain.Entities;
using KursovaWork.Infrastructure.Services.Entities;
using KursovaWork.Infrastructure.Unit.Tests.Fakers;
using Moq;

namespace KursovaWork.Infrastructure.Unit.Tests.Services.Entities;

public class CardServiceTests
{
    private readonly Mock<ICardRepository> _mockCardRepository;
    private readonly Mock<IIdRetriever> _mockIdRetriever;
    private readonly ICardService _cardService;
    private readonly CardFaker _cardFaker;
    public CardServiceTests()
    {
        _mockCardRepository = new Mock<ICardRepository>();
        _mockIdRetriever = new Mock<IIdRetriever>();
        _cardService = new CardService(_mockCardRepository.Object, _mockIdRetriever.Object);

        _cardFaker = new CardFaker();
    }

    [Fact]
    public void GetById_EntityExists_ShouldReturnCorrectCard()
    {
        // Arrange
        var expectedCard = _cardFaker.Generate();
        _mockCardRepository.Setup(repo => repo.GetById(expectedCard.UserId)).Returns(expectedCard);

        // Act
        var result = _cardService.GetById(expectedCard.UserId);

        // Assert
        result.Should().BeEquivalentTo(expectedCard);
    }

    [Fact]
    public void GetById_EntityNotExists_ShouldReturnNull()
    {
        // Arrange
        int id = 1;
        _mockCardRepository.Setup(repo => repo.GetById(1)).Returns<Card?>(null);

        // Act
        var result = _cardService.GetById(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetByLoggedInUser_EntityExists_ShouldReturnCorrectCard()
    {
        // Arrange
        var expectedCard = _cardFaker.Generate();
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(expectedCard.UserId);
        _mockCardRepository.Setup(repo => repo.GetById(expectedCard.UserId)).Returns(expectedCard);

        // Act
        var result = _cardService.GetByLoggedInUser();

        // Assert
        result.Should().BeEquivalentTo(expectedCard);
    }

    [Fact]
    public void GetByLoggedInUser_EntityNotExists_ShouldReturnNull()
    {
        // Arrange
        var id = 0;
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(id);
        _mockCardRepository.Setup(repo => repo.GetById(id)).Returns<Card?>(null);

        // Act
        var result = _cardService.GetByLoggedInUser();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void AddCard_ShouldSucceed()
    {
        // Arrange
        var cardToAdd = _cardFaker.Generate();
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(cardToAdd.UserId);

        // Act
        _cardService.AddCard(cardToAdd);

        // Assert
        _mockCardRepository.Verify(repo => repo.Add(cardToAdd), Times.Once);
    }

    [Fact]
    public void UpdateCard_ShouldSucceed()
    {
        // Arrange
        var cardToUpdate = _cardFaker.Generate();

        // Act
        _cardService.UpdateCard(cardToUpdate);

        // Assert
        _mockCardRepository.Verify(repo => repo.Update(cardToUpdate), Times.Once);
    }

    [Fact]
    public void DeleteCard_EntityExists_ShouldSucceed()
    {
        // Arrange
        var cardToDelete = _cardFaker.Generate();
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(cardToDelete.UserId);
        _mockCardRepository.Setup(repo => repo.GetById(cardToDelete.UserId)).Returns(cardToDelete);

        // Act
        _cardService.DeleteCard();

        // Assert
        _mockCardRepository.Verify(repo => repo.Delete(cardToDelete), Times.Once);
    }

    [Fact]
    public void DeleteCard_EntityNotExists_ShouldFail()
    {
        // Arrange
        var loggedInUserId = _cardFaker.Generate().UserId;
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(loggedInUserId);
        _mockCardRepository.Setup(repo => repo.GetById(loggedInUserId)).Returns<Card?>(null);

        // Act
        _cardService.DeleteCard();

        // Assert
        _mockCardRepository.Verify(repo => repo.Delete(It.IsAny<Card>()), Times.Never);
    }

    [Fact]
    public void GetAllCards_ShouldReturnAllCards()
    {
        // Arrange
        var expectedCards = _cardFaker.Generate(5);
        _mockCardRepository.Setup(repo => repo.GetAll()).Returns(expectedCards);

        // Act
        var result = _cardService.GetAllCards();

        // Assert
        result.Should().BeEquivalentTo(expectedCards);
    }

    [Fact]
    public void CardExists_EntityExists_ShouldReturnTrue()
    {
        // Arrange
        int loggedInUserId = 1;
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(loggedInUserId);
        _mockCardRepository.Setup(repo => repo.Exists(loggedInUserId)).Returns(true);

        // Act
        var result = _cardService.CardExists();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CardExists_EntityNotExists_ShouldReturnFalse()
    {
        // Arrange
        int id = 0;
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(id);
        _mockCardRepository.Setup(repo => repo.Exists(id)).Returns(false);

        // Act
        var result = _cardService.CardExists();

        // Assert
        result.Should().BeFalse();
    }
}
