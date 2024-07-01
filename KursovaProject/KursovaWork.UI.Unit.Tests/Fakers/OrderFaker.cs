using Bogus;
using KursovaWork.Domain.Entities;

namespace KursovaWork.UI.Unit.Tests.Fakers;

public sealed class OrderFaker : Faker<Order>
{
    public OrderFaker()
    {
        var carFaker = new CarFaker();
        var userFaker = new UserFaker();

        var car = carFaker.Generate();
        var user = userFaker.Generate();

        RuleFor(c => c.Id, f => f.IndexGlobal);
        RuleFor(o => o.CarId, f => car.Id);
        RuleFor(o => o.UserId, f => user.Id);
        RuleFor(o => o.Price, f => f.Random.Decimal(1000, 50000));
        RuleFor(o => o.OrderDate, f => f.Date.Past());

        FinishWith((f, o) =>
        {
            o.Car = car;
            o.User = user;
        });

        UseSeed(1994);
    }
}