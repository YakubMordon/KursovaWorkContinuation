using FluentAssertions;
using KursovaWork.Domain.Entities;
using KursovaWork.Domain.Models;
using KursovaWork.Infrastructure.Mappers.Entities;

namespace KursovaWork.Infrastructure.Unit.Tests.Mappers.Entities;

public class CardMapperTests
{
    private readonly CardMapper _mapper;

    public CardMapperTests()
    {
        _mapper = new CardMapper();
    }

    [Fact]
    public void EntityToModel_ShouldMapCardToCreditCardViewModel()
    {
        // Arrange

        var entity = new Card
        {
            UserId = 1,
            CardNumber = "1111 2222 3333 4444",
            CardHolderName = "Name",
            ExpirationMonth = "11",
            ExpirationYear = "24",
            Cvv = "123"
        };

        // Act

        var actual = _mapper.EntityToModel(entity);

        // Assert

        actual.UserId.Should().Be(entity.UserId);

        actual.CardNumber.Should().Be(entity.CardNumber);

        actual.CardHolderName.Should().Be(entity.CardHolderName);

        actual.ExpirationMonth.Should().Be(entity.ExpirationMonth);

        actual.ExpirationYear.Should().Be(entity.ExpirationYear);

        actual.Cvv.Should().Be(entity.Cvv);
    }

    [Fact]
    public void ModelToEntity_ShouldMapCreditCardViewModelToCard()
    {
        // Arrange

        var model = new CreditCardViewModel
        {
            UserId = 1,
            CardNumber = "1111 2222 3333 4444",
            CardHolderName = "Name",
            ExpirationMonth = "11",
            ExpirationYear = "24",
            Cvv = "123"
        };

        // Act

        var actual = _mapper.ModelToEntity(model);

        // Assert

        actual.UserId.Should().Be(model.UserId);

        actual.CardNumber.Should().Be(model.CardNumber);

        actual.CardHolderName.Should().Be(model.CardHolderName);

        actual.ExpirationMonth.Should().Be(model.ExpirationMonth);

        actual.ExpirationYear.Should().Be(model.ExpirationYear);

        actual.Cvv.Should().Be(model.Cvv);

        actual.User.Should().BeNull();
    }
}
