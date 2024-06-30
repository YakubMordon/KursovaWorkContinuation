using Bogus;
using KursovaWork.Domain.Entities;

namespace KursovaWork.Infrastructure.Unit.Tests.Fakers;

public sealed class CardFaker : Faker<Card>
{
    public CardFaker()
    {
        RuleFor(c => c.UserId, f => f.IndexGlobal);
        RuleFor(c => c.CardNumber, f => f.Finance.CreditCardNumber());
        RuleFor(c => c.CardHolderName, f => f.Name.FullName());
        RuleFor(c => c.ExpirationMonth, f => f.Random.Number(1, 12).ToString());
        RuleFor(c => c.ExpirationYear, f => f.Random.Number(24, 30).ToString());
        RuleFor(c => c.Cvv, f => f.Finance.CreditCardCvv());

        UseSeed(1994);
    }
}