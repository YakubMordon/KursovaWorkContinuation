using Bogus;
using KursovaWork.Domain.Entities.Car;

namespace KursovaWork.UI.Unit.Tests.Fakers;

public sealed class CarFaker : Faker<Car>
{
    public CarFaker()
    {
        RuleFor(c => c.Id, f => f.IndexGlobal);
        RuleFor(c => c.Make, f => f.Vehicle.Manufacturer());
        RuleFor(c => c.Model, f => f.Vehicle.Model());
        RuleFor(c => c.Year, f => f.Random.Number(1900, 2100));
        RuleFor(c => c.Price, f => f.Random.Decimal(100000, 500000));
        RuleFor(c => c.Description, f => f.Commerce.ProductDescription());

        FinishWith((f, c) =>
        {
            c.Detail = new CarDetail
            {
                Id = f.IndexGlobal,
                CarId = c.Id,
                FuelType = f.Vehicle.Fuel(),
                Color = "Black",
                Transmission = f.Random.Bool() ? "Automatic" : "Manual"
            };
        });

        UseSeed(1994);
    }
}