using FluentAssertions;
using KursovaWork.Domain.Entities.Car;
using KursovaWork.Infrastructure.Repositories;
using KursovaWork.Infrastructure.Unit.Tests.Fakers;
using Microsoft.EntityFrameworkCore;

namespace KursovaWork.Infrastructure.Unit.Tests.Repositories;

public class CarRepositoryTests : IDisposable, IAsyncDisposable
{
    private DbContextOptions<CarSaleContext> _dbContextOptions;
    private CarSaleContext _context;
    private CarRepository _carRepository;
    private CarFaker _carFaker;

    public CarRepositoryTests()
    {
        _carFaker = new CarFaker();

        _dbContextOptions = new DbContextOptionsBuilder<CarSaleContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_Cars")
            .Options;

        _context = new CarSaleContext(_dbContextOptions);
        _carRepository = new CarRepository(_context);
    }

    [Fact]
    public void Add_DataIsCorrect_ShouldAddCar()
    {
        // Arrange

        var entity = _carFaker.Generate();

        // Act

        _carRepository.Add(entity);

        // Assert

        _context.Cars.Should().Contain(entity);

        _carRepository.Delete(entity);
    }

    [Fact]
    public void Add_DataIsNotCorrect_ShouldNotAddCar()
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
    public void Delete_CarExists_ShouldDeleteCar()
    {
        // Arrange

        var entity = _carFaker.Generate();

        // Act

        _carRepository.Add(entity);

        _carRepository.Delete(entity);

        // Assert

        _context.Cars.Should().NotContain(entity);
    }

    [Fact]
    public void Delete_CarNotExists_ShouldNotDeleteCar()
    {
        // Arrange

        var entity = _carFaker.Generate();

        // Act

        Action removal = () => _carRepository.Delete(entity);

        // Assert

        removal.Should().Throw<DbUpdateConcurrencyException>();
    }

    [Fact]
    public void Update_DataIsCorrect_ShouldUpdateCar()
    {
        // Arrange

        var entity = _carFaker.Generate();

        // Act

        _carRepository.Add(entity);

        entity.Make = "NEW_TEST_MAKE";

        _carRepository.Update(entity);

        // Assert

        _context.Cars.Should().Contain(entity);

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
    public void GetById_CarExists_ShouldReturnCar()
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
    public void GetById_CarNotExists_ShouldReturnNull()
    {
        // Act

        var actual = _carRepository.GetById(10);

        // Assert

        actual.Should().BeNull();
    }

    [Fact]
    public void GetByCarInfo_CarExists_ShouldReturnCar()
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
    public void GetByCarInfo_CarNotExists_ShouldReturnNull()
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