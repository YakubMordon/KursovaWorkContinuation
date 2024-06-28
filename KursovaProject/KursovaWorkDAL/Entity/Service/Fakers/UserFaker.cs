using Bogus;
using KursovaWorkDAL.Entity.Entities;

namespace KursovaWorkDAL.Entity.Service.Fakers;

public sealed class UserFaker : Faker<User>
{
    public UserFaker()
    {
        UseSeed(1994);
        RuleFor(entity => entity.Id, faker => faker.IndexFaker);
        RuleFor(entity => entity.FirstName, faker => faker.Person.FirstName);
        RuleFor(entity => entity.LastName, faker => faker.Person.LastName);
        RuleFor(entity => entity.Email, faker => faker.Person.Email);
        RuleFor(entity => entity.Password, faker => faker.Internet.Password());
        RuleFor(entity => entity.ConfirmPassword, (faker, user) => user.Password);
    }
}