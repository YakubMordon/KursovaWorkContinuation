using FluentAssertions;
using KursovaWork.Application.Contracts.Repositories;
using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Application.Contracts.Services.Helpers.Transient;
using KursovaWork.Domain.Entities;
using KursovaWork.Infrastructure.Services.DB;
using KursovaWork.Infrastructure.Services.DB.Fakers;
using KursovaWork.Infrastructure.Services.Entities;
using Moq;

namespace KursovaWork.Infrastructure.Unit.Tests.Services.Entities;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IIdRetriever> _mockIdRetriever;
    private readonly IUserService _userService;

    private readonly UserFaker _userFaker;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockIdRetriever = new Mock<IIdRetriever>();
        _userService = new UserService(_mockUserRepository.Object, _mockIdRetriever.Object);

        _userFaker = new UserFaker();
    }

    [Fact]
    public void AddUser_ShouldSucceed()
    {
        // Arrange
        var userToAdd = _userFaker.Generate();

        // Act
        _userService.AddUser(userToAdd);

        // Assert
        _mockUserRepository.Verify(repo => repo.Add(userToAdd), Times.Once);
    }

    [Fact]
    public void DeleteUser_ShouldSucceed()
    {
        // Arrange
        var userToDelete = _userFaker.Generate();

        // Act
        _userService.DeleteUser(userToDelete);

        // Assert
        _mockUserRepository.Verify(repo => repo.Delete(userToDelete), Times.Once);
    }

    [Fact]
    public void GetAllUsers_ShouldReturnAllUsers()
    {
        // Arrange
        var expectedUsers = _userFaker.Generate(5);
        _mockUserRepository.Setup(repo => repo.GetAll()).Returns(expectedUsers);

        // Act
        var result = _userService.GetAllUsers();

        // Assert
        result.Should().BeEquivalentTo(expectedUsers);
    }

    [Fact]
    public void GetLoggedInUser_UserLoggedIn_ShouldReturnLoggedInUser()
    {
        // Arrange
        var expectedUser = _userFaker.Generate();
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(expectedUser.Id);
        _mockUserRepository.Setup(repo => repo.GetById(expectedUser.Id)).Returns(expectedUser);

        // Act
        var result = _userService.GetLoggedInUser();

        // Assert
        result.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public void GetLoggedInUser_UserNotLoggedIn_ShouldReturnNull()
    {
        // Arrange
        var expectedUser = _userFaker.Generate();
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(0);
        _mockUserRepository.Setup(repo => repo.GetById(0)).Returns<User?>(null);

        // Act
        var result = _userService.GetLoggedInUser();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetUserById_ShouldReturnUserById()
    {
        // Arrange
        var expectedUser = _userFaker.Generate();
        _mockUserRepository.Setup(repo => repo.GetById(expectedUser.Id)).Returns(expectedUser);

        // Act
        var result = _userService.GetUserById(expectedUser.Id);

        // Assert
        result.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public void GetUserByEmail_ShouldReturnUserByEmail()
    {
        // Arrange
        var expectedUser = _userFaker.Generate();
        _mockUserRepository.Setup(repo => repo.GetByEmail(expectedUser.Email)).Returns(expectedUser);

        // Act
        var result = _userService.GetUserByEmail(expectedUser.Email);

        // Assert
        result.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public void UpdatePasswordOfUser_ShouldSucceed()
    {
        // Arrange
        var userToUpdate = _userFaker.Generate();
        var newPassword = "newpassword";
        var confirmPassword = "newpassword";

        // Act
        _userService.UpdatePasswordOfUser(newPassword, confirmPassword, userToUpdate);

        // Assert
        userToUpdate.Password.Should().Be(newPassword);
        _mockUserRepository.Verify(repo => repo.Update(userToUpdate), Times.Once);
    }

    [Fact]
    public void UpdateUser_ShouldSucceed()
    {
        // Arrange
        var userToUpdate = _userFaker.Generate();

        // Act
        _userService.UpdateUser(userToUpdate);

        // Assert
        _mockUserRepository.Verify(repo => repo.Update(userToUpdate), Times.Once);
    }

    [Fact]
    public void ValidateUser_DataIsCorrect_ShouldReturnValidUser()
    {
        // Arrange
        var users = _userFaker.Generate(3);
        var userId = users.First().Id;
        var userEmail = users.First().Email;
        var password = users.First().Password;

        users[0].Password = Encrypter.HashPassword(password);

        _mockUserRepository.Setup(repo => repo.GetAll()).Returns(users);

        // Act
        var result = _userService.ValidateUser(userEmail, password);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(userId); // Ensure non-null assertion due to .FirstOrDefault()
    }

    [Fact]
    public void ValidateUser_DataIsNotCorrect_ShouldReturnNull()
    {
        // Arrange
        var users = _userFaker.Generate(3);
        var userId = users.First().Id;
        var userEmail = users.First().Email;
        var password = users.First().Password;

        users[0].Password = Encrypter.HashPassword(password);

        _mockUserRepository.Setup(repo => repo.GetAll()).Returns(users);

        // Act
        var result = _userService.ValidateUser(userEmail, "IncorrectPassword");

        // Assert
        result.Should().BeNull();
    }
}
