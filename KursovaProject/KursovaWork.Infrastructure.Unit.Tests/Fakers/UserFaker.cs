using Bogus;
using KursovaWork.Domain.Entities;

namespace KursovaWork.Infrastructure.Unit.Tests.Fakers;

public sealed class UserFaker : Faker<User>
{
    public UserFaker()
    {
        RuleFor(u => u.Id, f => f.IndexGlobal);
        RuleFor(u => u.FirstName, f => f.Person.FirstName);
        RuleFor(u => u.LastName, f => f.Person.LastName);
        RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName));
        RuleFor(u => u.Password, f => f.Internet.Password(8, false, "", "Aa1!"));
        RuleFor(u => u.ConfirmPassword, (f, u) => u.Password);
        RuleFor(u => u.CreatedAt, f => f.Date.Past());

        UseSeed(1994);
    }
}