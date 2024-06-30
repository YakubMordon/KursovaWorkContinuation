using FluentAssertions;
using KursovaWork.Domain.Entities;
using KursovaWork.Infrastructure.Repositories;
using KursovaWork.Infrastructure.Unit.Tests.Fakers;
using Microsoft.EntityFrameworkCore;

namespace KursovaWork.Infrastructure.Unit.Tests.Repositories;

public class UserRepositoryTests
{
    private DbContextOptions<CarSaleContext> _dbContextOptions;
    private CarSaleContext _context;
    private UserRepository _userRepository;
    private UserFaker _userFaker;

    public UserRepositoryTests()
    {
        _userFaker = new UserFaker();

        _dbContextOptions = new DbContextOptionsBuilder<CarSaleContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_Users")
            .Options;

        _context = new CarSaleContext(_dbContextOptions);
        _userRepository = new UserRepository(_context);
    }

    [Fact]
    public void Add_DataIsCorrect_ShouldAddUser()
    {
        // Arrange

        var entity = _userFaker.Generate();

        // Act

        _userRepository.Add(entity);

        // Assert

        _context.Users.Should().Contain(entity);

        _userRepository.Delete(entity);
    }

    [Fact]
    public void Add_DataIsNotCorrect_ShouldNotAddCar()
    {
        // Arrange

        var entity = new User
        {
            Id = 2,
        };

        // Act

        Action adding = () => _userRepository.Add(entity);

        // Assert

        adding.Should().Throw<DbUpdateException>();
    }

    [Fact]
    public void Delete_UserExists_ShouldDeleteUser()
    {
        // Arrange

        var entity = _userFaker.Generate();

        // Act

        _userRepository.Add(entity);

        _userRepository.Delete(entity);

        // Assert

        _context.Users.Should().NotContain(entity);
    }

    [Fact]
    public void Delete_UserNotExists_ShouldNotDeleteUser()
    {
        // Arrange

        var entity = _userFaker.Generate();

        // Act

        Action removal = () => _userRepository.Delete(entity);

        // Assert

        removal.Should().Throw<DbUpdateConcurrencyException>();
    }

    [Fact]
    public void Update_DataIsCorrect_ShouldUpdateUser()
    {
        // Arrange

        var entity = _userFaker.Generate();

        // Act

        _userRepository.Add(entity);

        entity.FirstName = "NewFirstName";

        _userRepository.Update(entity);

        // Assert

        _context.Users.Should().Contain(entity);

        _userRepository.Delete(entity);
    }    

    [Fact]
    public void GetAll_ShouldGetAllUsers()
    {
        // Arrange

        var list = _userFaker.Generate(10);

        // Act

        _context.Users.AddRange(list);

        _context.SaveChanges();

        var actual = _userRepository.GetAll();

        // Assert

        actual.Should().BeEquivalentTo(list);

        _context.Users.RemoveRange(list);

        _context.SaveChanges();
    }

    [Fact]
    public void GetById_UserExists_ShouldReturnUser()
    {
        // Arrange

        var entity = _userFaker.Generate();

        // Act

        _userRepository.Add(entity);

        var actual = _userRepository.GetById(entity.Id);

        // Assert

        actual.Should().BeEquivalentTo(entity);

        _userRepository.Delete(entity);
    }

    [Fact]
    public void GetById_UserNotExists_ShouldReturnNull()
    {
        // Act

        var actual = _userRepository.GetById(10);

        // Assert

        actual.Should().BeNull();
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