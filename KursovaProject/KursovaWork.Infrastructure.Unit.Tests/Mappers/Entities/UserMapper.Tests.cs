using FluentAssertions;
using KursovaWork.Domain.Entities;
using KursovaWork.Domain.Models;
using KursovaWork.Infrastructure.Mappers.Entities;

namespace KursovaWork.Infrastructure.Unit.Tests.Mappers.Entities;

public class UserMapperTests
{
    private readonly UserMapper _mapper;

    public UserMapperTests()
    {
        _mapper = new UserMapper();
    }

    [Fact]
    public void EntityToModel_ShouldMapUserToSignUpViewModel()
    {
        // Arrange

        var entity = new User
        {
            Id = 1,
            FirstName = "FirstName",
            LastName = "LastName",
            Email = "FirstName@email",
            Password = "Password",
            ConfirmPassword = "Password",
            CreatedAt = DateTime.Now
        };

        // Act

        var actual = _mapper.EntityToModel(entity);

        // Assert

        actual.FirstName.Should().Be(entity.FirstName);

        actual.LastName.Should().Be(entity.LastName);

        actual.Email.Should().Be(entity.Email);

        actual.Password.Should().Be(entity.Password);

        actual.ConfirmPassword.Should().Be(entity.ConfirmPassword);
    }

    [Fact]
    public void ModelToEntity_ShouldMapSignUpViewModelToUser()
    {
        // Arrange

        var model = new SignUpViewModel
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Email = "FirstName@email",
            Password = "Password",
            ConfirmPassword = "Password"
        };

        // Act

        var actual = _mapper.ModelToEntity(model);

        // Assert

        actual.Id.Should().Be(default);

        actual.FirstName.Should().Be(model.FirstName);

        actual.LastName.Should().Be(model.LastName);

        actual.Email.Should().Be(model.Email);

        actual.Password.Should().Be(model.Password);

        actual.ConfirmPassword.Should().Be(model.ConfirmPassword);

        actual.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(1));

        actual.Orders.Should().BeNull();

        actual.CreditCard.Should().BeNull();
    }
}