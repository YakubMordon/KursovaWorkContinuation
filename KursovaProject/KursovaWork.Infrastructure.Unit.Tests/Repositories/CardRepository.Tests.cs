using FluentAssertions;
using KursovaWork.Domain.Entities;
using KursovaWork.Infrastructure.Repositories;
using KursovaWork.Infrastructure.Unit.Tests.Fakers;
using Microsoft.EntityFrameworkCore;

namespace KursovaWork.Infrastructure.Unit.Tests.Repositories;

public class CardRepositoryTests : IDisposable, IAsyncDisposable
{
    private DbContextOptions<CarSaleContext> _dbContextOptions;
    private CarSaleContext _context;
    private CardRepository _cardRepository;
    private CardFaker _cardFaker;

    public CardRepositoryTests()
    {
        _cardFaker = new CardFaker();

        _dbContextOptions = new DbContextOptionsBuilder<CarSaleContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_Cards")
            .Options;

        _context = new CarSaleContext(_dbContextOptions);
        _cardRepository = new CardRepository(_context);
    }

    [Fact]
    public void Add_IfDataIsCorrect_ShouldAddCard()
    {
        // Arrange

        var entity = _cardFaker.Generate();

        // Act

        _cardRepository.Add(entity);

        // Assert

        _context.Cards.Should().Contain(card => card.UserId == entity.UserId);

        _cardRepository.Delete(entity);
    }

    [Fact]
    public void Add_IfDataIsNotCorrect_ShouldNotAddCard()
    {
        // Arrange

        var entity = new Card
        {
            UserId = 2,
        };

        // Act

        Action adding = () => _cardRepository.Add(entity);

        // Assert

        adding.Should().Throw<DbUpdateException>();
    }

    [Fact]
    public void Delete_IfCardExists_ShouldDeleteCard()
    {
        // Arrange

        var entity = _cardFaker.Generate();

        // Act

        _cardRepository.Add(entity);

        _cardRepository.Delete(entity);

        // Assert

        _context.Cards.Should().NotContain(card => card.UserId == entity.UserId);
    }

    [Fact]
    public void Delete_IfCardNotExists_ShouldNotDeleteCard()
    {
        // Arrange

        var entity = _cardFaker.Generate();

        // Act

        Action removal = () => _cardRepository.Delete(entity);

        // Assert

        removal.Should().Throw<DbUpdateConcurrencyException>();
    }

    [Fact]
    public void Update_IfDataIsCorrect_ShouldUpdateCard()
    {
        // Arrange

        var entity = _cardFaker.Generate();

        // Act

        _cardRepository.Add(entity);

        entity.CardHolderName = "123";

        _cardRepository.Update(entity);

        // Assert

        _context.Cards.Should()
            .Contain(card => card.UserId == entity.UserId)
            .Which.Should().BeEquivalentTo(entity);

        _cardRepository.Delete(entity);
    }

    [Fact]
    public void GetAll_ShouldGetAllCards()
    {
        // Arrange

        var list = _cardFaker.Generate(10);

        // Act

        _context.Cards.AddRange(list);

        _context.SaveChanges();

        var actual = _cardRepository.GetAll();

        // Assert

        actual.Should().BeEquivalentTo(list);

        _context.Cards.RemoveRange(list);

        _context.SaveChanges();
    }

    [Fact]
    public void GetById_IfCardExists_ShouldReturnCard()
    {
        // Arrange

        var entity = _cardFaker.Generate();

        // Act

        _cardRepository.Add(entity);

        var actual = _cardRepository.GetById(entity.UserId);

        // Assert

        actual.Should().BeEquivalentTo(entity);

        _cardRepository.Delete(entity);
    }

    [Fact]
    public void GetById_IfCardNotExists_ShouldReturnNull()
    {
        // Act

        var actual = _cardRepository.GetById(10);

        // Assert

        actual.Should().BeNull();
    }

    [Fact]
    public void Exists_IfCardExists_ShouldReturnTrue()
    {
        // Arrange

        var entity = _cardFaker.Generate();

        // Act

        _cardRepository.Add(entity);

        var actual = _cardRepository.Exists(entity.UserId);

        // Assert

        actual.Should().BeTrue();

        _cardRepository.Delete(entity);
    }

    [Fact]
    public void Exists_IfCardNotExists_ShouldReturnFalse()
    {
        // Act

        var actual = _cardRepository.Exists(12);

        // Assert

        actual.Should().BeFalse();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}