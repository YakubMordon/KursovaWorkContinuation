using Bogus;
using KursovaWork.Domain.Entities;

namespace KursovaWork.Infrastructure.Services.DB.Fakers;

/// <summary>
/// Faker for user entity.
/// </summary>
public sealed class UserFaker : Faker<User>
{
    /// <summary>
    /// Initializes instance of <see cref="UserFaker"/>.
    /// </summary>
    public UserFaker()
    {
        UseSeed(1994);
        RuleFor(entity => entity.FirstName, faker => faker.Person.FirstName);
        RuleFor(entity => entity.LastName, faker => faker.Person.LastName);
        RuleFor(entity => entity.Email, faker => faker.Person.Email);
        RuleFor(entity => entity.Password, faker => faker.Internet.Password());
        RuleFor(entity => entity.ConfirmPassword, (faker, user) => user.Password);
    }
}