using Bogus;
using FluentAssertions;
using KursovaWork.Domain.Entities;
using KursovaWork.Domain.Entities.Car;
using KursovaWork.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KursovaWork.Infrastructure.Unit.Tests.Repositories;

public class CarRepositoryTests : IDisposable, IAsyncDisposable
{
    private DbContextOptions<CarSaleContext> _dbContextOptions;
    private CarSaleContext _context;
    private CarRepository _carRepository;
    private Faker<Car> _carFaker;

    public CarRepositoryTests()
    {
        // Initialize Bogus faker
        _carFaker = new Faker<Car>()
            .RuleFor(c => c.Id, f => f.Random.Number(1, 100))
            .RuleFor(c => c.Make, f => f.Vehicle.Manufacturer())
            .RuleFor(c => c.Model, f => f.Vehicle.Model())
            .RuleFor(c => c.Year, f => f.Random.Number(1900, 2100))
            .RuleFor(c => c.Price, f => f.Random.Decimal(100000, 500000))
            .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
            .UseSeed(1994);

        // Налаштовуємо In-Memory базу даних
        _dbContextOptions = new DbContextOptionsBuilder<CarSaleContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_Cars")
            .Options;

        _context = new CarSaleContext(_dbContextOptions);
        _carRepository = new CarRepository(_context);
    }

    [Fact]
    public void Add_IfDataIsCorrect_ShouldAddCar()
    {
        // Arrange

        var entity = _carFaker.Generate();

        // Act

        _carRepository.Add(entity);

        // Assert

        _context.Cars.Should().Contain(car => car.Id == entity.Id);

        _carRepository.Delete(entity);
    }

    [Fact]
    public void Add_IfDataIsNotCorrect_ShouldNotAddCar()
    {
        // Arrange

        var entity = new Car
        {
            Id = 2,
        };

        // Act

        Action adding = () => _carRepository.Add(entity);

        // Assert

        adding.Should().Throw<DbUpdateException>();
    }

    [Fact]
    public void Delete_IfCarExists_ShouldDeleteCar()
    {
        // Arrange

        var entity = _carFaker.Generate();

        // Act

        _carRepository.Add(entity);

        _carRepository.Delete(entity);

        // Assert

        _context.Cars.Should().NotContain(car => car.Id == entity.Id);
    }

    [Fact]
    public void Delete_IfCarNotExists_ShouldNotDeleteCar()
    {
        // Arrange

        var entity = _carFaker.Generate();

        // Act

        Action removal = () => _carRepository.Delete(entity);

        // Assert

        removal.Should().Throw<DbUpdateConcurrencyException>();
    }

    [Fact]
    public void Update_IfDataIsCorrect_ShouldUpdateCar()
    {
        // Arrange

        var entity = _carFaker.Generate();

        // Act

        _carRepository.Add(entity);

        entity.Make = "NEW_TEST_MAKE";

        _carRepository.Update(entity);

        // Assert

        _context.Cars.Should()
            .Contain(car => car.Id == entity.Id)
            .Which.Make
                    .Should()
                    .Be(entity.Make);

        _carRepository.Delete(entity);
    }

    [Fact]
    public void GetAll_ShouldGetAllCars()
    {
        // Arrange

        var list = _carFaker.Generate(10);

        // Act

        _context.Cars.AddRange(list);

        _context.SaveChanges();

        var actual = _carRepository.GetAll();

        // Assert

        actual.Should().BeEquivalentTo(list);

        _context.Cars.RemoveRange(list);

        _context.SaveChanges();
    }

    [Fact]
    public void GetById_IfCarExists_ShouldReturnCar()
    {
        // Arrange

        var entity = _carFaker.Generate();

        // Act

        _carRepository.Add(entity);

        var actual = _carRepository.GetById(entity.Id);

        // Assert

        actual.Should().BeEquivalentTo(entity);

        _carRepository.Delete(entity);
    }

    [Fact]
    public void GetById_IfCarNotExists_ShouldReturnNull()
    {
        // Act

        var actual = _carRepository.GetById(10);

        // Assert

        actual.Should().BeNull();
    }

    [Fact]
    public void GetByCarInfo_IfCarExists_ShouldReturnCar()
    {
        // Arrange

        var entity = _carFaker.Generate();

        // Act

        _carRepository.Add(entity);

        var actual = _carRepository.GetByCarInfo(entity.Make, entity.Model, entity.Year);

        // Assert

        actual.Should().BeEquivalentTo(entity);

        _carRepository.Delete(entity);
    }

    [Fact]
    public void GetByCarInfo_IfCarNotExists_ShouldReturnNull()
    {
        // Act

        var actual = _carRepository.GetByCarInfo("Make", "Model", 2020);

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