using FluentAssertions;
using KursovaWork.Application.Contracts.Repositories;
using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Domain.Entities.Car;
using KursovaWork.Infrastructure.Services.Entities;
using KursovaWork.Infrastructure.Unit.Tests.Fakers;
using Moq;

namespace KursovaWork.Infrastructure.Unit.Tests.Services.Entities;

public class CarServiceTests
{
    private readonly Mock<ICarRepository> _mockCarRepository;
    private readonly ICarService _carService;
    private readonly CarFaker _carFaker;
    public CarServiceTests()
    {
        _mockCarRepository = new Mock<ICarRepository>();
        _carService = new CarService(_mockCarRepository.Object);

        _carFaker = new CarFaker();
    }

    [Fact]
    public void GetById_EntityExists_ShouldReturnCorrectCar()
    {
        // Arrange
        var expectedCar = _carFaker.Generate();
        _mockCarRepository.Setup(repo => repo.GetById(expectedCar.Id)).Returns(expectedCar);

        // Act
        var result = _carService.GetCarById(expectedCar.Id);

        // Assert
        result.Should().BeEquivalentTo(expectedCar);
    }

    [Fact]
    public void GetById_EntityNotExists_ShouldReturnNull()
    {
        // Arrange
        int id = 1;
        _mockCarRepository.Setup(repo => repo.GetById(1)).Returns<Car?>(null);

        // Act
        var result = _carService.GetCarById(id);

        // Assert
        result.Should().BeNull();
    }    
    [Fact]
    public void GetByCarInfo_EntityExists_ShouldReturnCorrectCar()
    {
        // Arrange
        var expectedCar = _carFaker.Generate();
        _mockCarRepository.Setup(repo => repo.GetByCarInfo(expectedCar.Make, expectedCar.Model, expectedCar.Year)).Returns(expectedCar);

        // Act
        var result = _carService.GetCarByInfo(expectedCar.Make, expectedCar.Model, expectedCar.Year);

        // Assert
        result.Should().BeEquivalentTo(expectedCar);
    }

    [Fact]
    public void GetByCarInfo_EntityNotExists_ShouldReturnNull()
    {
        // Arrange
        var expectedCar = _carFaker.Generate();
        _mockCarRepository.Setup(repo => repo.GetByCarInfo(expectedCar.Make, expectedCar.Model,expectedCar.Year)).Returns<Car?>(null);

        // Act
        var result = _carService.GetCarByInfo(expectedCar.Make, expectedCar.Model, expectedCar.Year);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void AddCar_ShouldSucceed()
    {
        // Arrange
        var carToAdd = _carFaker.Generate();

        // Act
        _carService.AddCar(carToAdd);

        // Assert
        _mockCarRepository.Verify(repo => repo.Add(carToAdd), Times.Once);
    }

    [Fact]
    public void UpdateCar_ShouldSucceed()
    {
        // Arrange
        var carToUpdate = _carFaker.Generate();

        // Act
        _carService.UpdateCar(carToUpdate);

        // Assert
        _mockCarRepository.Verify(repo => repo.Update(carToUpdate), Times.Once);
    }

    [Fact]
    public void DeleteCar_ShouldSucceed()
    {
        // Arrange
        var carToDelete = _carFaker.Generate();

        // Act
        _carService.DeleteCar(carToDelete);

        // Assert
        _mockCarRepository.Verify(repo => repo.Delete(carToDelete), Times.Once);
    }

    [Fact]
    public void GetAllCars_ShouldReturnAllCars()
    {
        // Arrange
        var expectedCars = _carFaker.Generate(5);
        _mockCarRepository.Setup(repo => repo.GetAll()).Returns(expectedCars);

        // Act
        var result = _carService.GetAllCars();

        // Assert
        result.Should().BeEquivalentTo(expectedCars);
    }

    [Fact]
    public void SortByAlphabet_ShouldSortListCorrectly()
    {
        // Arrange
        var list = _carFaker.Generate(5);

        // Act
        var actual = _carService.SortByAlphabet(list);

        list = list.OrderBy(o => o.Make + o.Model).ToList();

        // Assert
        actual.Should().BeEquivalentTo(list);
    }    
    
    [Fact]
    public void SortByNovelty_ShouldSortListCorrectly()
    {
        // Arrange
        var list = _carFaker.Generate(5);

        // Act
        var actual = _carService.SortByNovelty(list);

        list = list.OrderByDescending(o => o.Year).ToList();

        // Assert
        actual.Should().BeEquivalentTo(list);
    }    
    
    [Theory]
    [InlineData("cheap")]
    [InlineData("expensive")]
    public void SortByPrice_ShouldSortListCorrectly(string param)
    {
        // Arrange
        var list = _carFaker.Generate(5);

        // Act
        var actual = _carService.SortByPrice(list, param);

        list = param.Equals("cheap")  
            ? list.OrderBy(o => o.Price).ToList() 
            : list.OrderByDescending(o => o.Price).ToList();

        // Assert
        actual.Should().BeEquivalentTo(list);
    }

    [Fact]
    public void Filtering_ShouldFilterByPriceFrom()
    {
        // Arrange
        var cars = _carFaker.Generate(5);
        var priceFrom = 20000;
        _mockCarRepository.Setup(repo => repo.GetAll()).Returns(cars);

        // Act
        var result = _carService.Filtering(priceFrom, null, null, null, null, null, null);

        var expected = cars.Where(o => o.Price >= priceFrom);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Filtering_ShouldFilterByPriceTo()
    {
        // Arrange
        var cars = _carFaker.Generate(5);
        var priceTo = 30000;
        _mockCarRepository.Setup(repo => repo.GetAll()).Returns(cars);

        // Act
        var result = _carService.Filtering(null, priceTo, null, null, null, null, null);

        var expected = cars.Where(o => o.Price <= priceTo);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Filtering_ShouldFilterByYearFrom()
    {
        // Arrange
        var cars = _carFaker.Generate(5);
        var yearFrom = 2010;
        _mockCarRepository.Setup(repo => repo.GetAll()).Returns(cars);

        // Act
        var result = _carService.Filtering(null, null, yearFrom, null, null, null, null);

        var expected = cars.Where(o => o.Year >= yearFrom);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Filtering_ShouldFilterByYearTo()
    {
        // Arrange
        var cars = _carFaker.Generate(5);
        var yearTo = 2015;
        _mockCarRepository.Setup(repo => repo.GetAll()).Returns(cars);

        // Act
        var result = _carService.Filtering(null, null, null, yearTo, null, null, null);

        var expected = cars.Where(o => o.Year <= yearTo);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Filtering_ShouldFilterBySelectedFuelTypes()
    {
        // Arrange
        var cars = _carFaker.Generate(5);
        var selectedFuelType = "Gasoline";
        cars[0].Detail.FuelType = selectedFuelType;
        _mockCarRepository.Setup(repo => repo.GetAll()).Returns(cars);

        // Act
        var result = _carService.Filtering(null, null, null, null, selectedFuelType, null, null);

        // Assert
        result.Should().OnlyContain(c => c.Detail.FuelType == selectedFuelType);
    }

    [Fact]
    public void Filtering_ShouldFilterBySelectedTransmissionTypes()
    {
        // Arrange
        var cars = _carFaker.Generate(5);
        var selectedTransmissionType = "Automatic";
        cars[0].Detail.Transmission = selectedTransmissionType;
        _mockCarRepository.Setup(repo => repo.GetAll()).Returns(cars);

        // Act
        var result = _carService.Filtering(null, null, null, null, null, selectedTransmissionType, null);

        // Assert
        result.Should().OnlyContain(c => c.Detail.Transmission == selectedTransmissionType);
    }

    [Fact]
    public void Filtering_ShouldFilterBySelectedMakes()
    {
        // Arrange
        var cars = _carFaker.Generate(5);
        var selectedMake = "Toyota";
        cars[0].Make = selectedMake;
        _mockCarRepository.Setup(repo => repo.GetAll()).Returns(cars);

        // Act
        var result = _carService.Filtering(null, null, null, null, null, null, selectedMake);

        var expected = cars.Where(o => o.Make.Equals(selectedMake));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
