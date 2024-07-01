using FluentAssertions;
using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Domain.Models;
using KursovaWork.UI.Controllers;
using KursovaWork.UI.Unit.Tests.Fakers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KursovaWork.UI.Unit.Tests.Controllers;

public class ModelListControllerTests
{
    private readonly Mock<ICarService> _mockCarService;
    private readonly ModelListController _controller;

    private readonly CarFaker _carFaker;

    public ModelListControllerTests()
    {
        _mockCarService = new Mock<ICarService>();
        _controller = new ModelListController(_mockCarService.Object);

        _carFaker = new CarFaker();
    }

    [Fact]
    public void ModelList_ReturnsCorrectView()
    {
        // Arrange
        var cars = _carFaker.Generate(5);
        _mockCarService.Setup(s => s.GetAllCars()).Returns(cars);

        // Act
        var result = _controller.ModelList() as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ModelList/ModelList.cshtml");

        var model = result.Model as FilterViewModel;
        model.Should().NotBeNull();
        model.Cars.Should().BeEquivalentTo(cars);
    }

    [Fact]
    public void SortByAlphabet_SortsListCorrectly()
    {
        // Arrange
        var unsortedCars = _carFaker.Generate(5);

        _mockCarService.Setup(s => s.GetAllCars()).Returns(unsortedCars);

        _controller.ModelList();

        var sortedCars = unsortedCars.OrderBy(o => o.Make + o.Model);

        _mockCarService.Setup(s => s.SortByAlphabet(unsortedCars)).Returns(sortedCars);

        // Act
        var result = _controller.SortByAlphabet() as PartialViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ModelList/_PartialModelList.cshtml");

        var model = result.Model as FilterViewModel;
        model.Should().NotBeNull();
        model.Cars.Should().BeEquivalentTo(sortedCars);
    }

    [Fact]
    public void SortByNovelty_SortsListCorrectly()
    {
        // Arrange
        var unsortedCars = _carFaker.Generate(5);

        _mockCarService.Setup(s => s.GetAllCars()).Returns(unsortedCars);

        _controller.ModelList();

        var sortedCars = unsortedCars.OrderByDescending(o => o.Year).ToList();

        _mockCarService.Setup(s => s.SortByNovelty(unsortedCars)).Returns(sortedCars);

        // Act
        var result = _controller.SortByNovelty() as PartialViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ModelList/_PartialModelList.cshtml");

        var model = result.Model as FilterViewModel;
        model.Should().NotBeNull();
        model.Cars.Should().BeEquivalentTo(sortedCars);
    }

    [Theory]
    [InlineData("cheap")]
    [InlineData("expensive")]
    public void SortByPrice_SortsListCorrectly(string param)
    {
        // Arrange
        var unsortedCars = _carFaker.Generate(5);

        _mockCarService.Setup(s => s.GetAllCars()).Returns(unsortedCars);

        _controller.ModelList();

        var sortedCars = param.Equals("cheap")
            ? unsortedCars.OrderBy(o => o.Price).ToList()
            : unsortedCars.OrderByDescending(o => o.Price).ToList();

        _mockCarService.Setup(s => s.SortByPrice(unsortedCars, param)).Returns(sortedCars);

        // Act
        var result = _controller.SortByPrice(param) as PartialViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ModelList/_PartialModelList.cshtml");

        var model = result.Model as FilterViewModel;
        model.Should().NotBeNull();
        model.Cars.Should().BeEquivalentTo(sortedCars);
    }

    [Fact]
    public void ApplyFilters_FiltersByPriceFrom()
    {
        // Arrange
        var unfilteredCars = _carFaker.Generate(5);

        var filter = new FilterViewModel
        {
            PriceFrom = 20000
        };

        _mockCarService.Setup(s => s.GetAllCars()).Returns(unfilteredCars);

        _controller.ModelList();

        var filteredCars = unfilteredCars.Where(o => o.Price >= filter.PriceFrom);

        _mockCarService.Setup(s => s.Filtering(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), 
            It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .Returns(filteredCars);

        // Act
        var result = _controller.ApplyFilters(filter) as PartialViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ModelList/_PartialModelList.cshtml");

        var model = result.Model as FilterViewModel;
        model.Should().NotBeNull();
        model.Cars.Should().BeEquivalentTo(filteredCars);
    }

    [Fact]
    public void ApplyFilters_FiltersByPriceTo()
    {
        // Arrange
        var unfilteredCars = _carFaker.Generate(5);

        var filter = new FilterViewModel
        {
            PriceTo = 30000
        };

        _mockCarService.Setup(s => s.GetAllCars()).Returns(unfilteredCars);

        _controller.ModelList();

        var filteredCars = unfilteredCars.Where(o => o.Price <= filter.PriceTo);

        _mockCarService.Setup(s => s.Filtering(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
            It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .Returns(filteredCars);

        // Act
        var result = _controller.ApplyFilters(filter) as PartialViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ModelList/_PartialModelList.cshtml");

        var model = result.Model as FilterViewModel;
        model.Should().NotBeNull();
        model.Cars.Should().BeEquivalentTo(filteredCars);
    }

    [Fact]
    public void ApplyFilters_FiltersFilterByYearFrom()
    {
        // Arrange
        var unfilteredCars = _carFaker.Generate(5);

        var filter = new FilterViewModel
        {
            YearFrom = 2010
        };

        _mockCarService.Setup(s => s.GetAllCars()).Returns(unfilteredCars);

        _controller.ModelList();

        var filteredCars = unfilteredCars.Where(o => o.Year >= filter.YearFrom);

        _mockCarService.Setup(s => s.Filtering(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
            It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .Returns(filteredCars);

        // Act
        var result = _controller.ApplyFilters(filter) as PartialViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ModelList/_PartialModelList.cshtml");

        var model = result.Model as FilterViewModel;
        model.Should().NotBeNull();
        model.Cars.Should().BeEquivalentTo(filteredCars);
    }

    [Fact]
    public void ApplyFilters_FiltersByYearTo()
    {
        // Arrange
        var unfilteredCars = _carFaker.Generate(5);

        var filter = new FilterViewModel
        {
            YearTo = 2015
        };

        _mockCarService.Setup(s => s.GetAllCars()).Returns(unfilteredCars);

        _controller.ModelList();

        var filteredCars = unfilteredCars.Where(o => o.Year >= filter.YearTo);

        _mockCarService.Setup(s => s.Filtering(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), 
            It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .Returns(filteredCars);

        // Act
        var result = _controller.ApplyFilters(filter) as PartialViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ModelList/_PartialModelList.cshtml");

        var model = result.Model as FilterViewModel;
        model.Should().NotBeNull();
        model.Cars.Should().BeEquivalentTo(filteredCars);
    }

    [Fact]
    public void ApplyFilters_FiltersBySelectedFuelTypes()
    {
        // Arrange
        var unfilteredCars = _carFaker.Generate(5);

        var filter = new FilterViewModel
        {
            SelectedFuelTypes = "Gasoline"
        };

        _mockCarService.Setup(s => s.GetAllCars()).Returns(unfilteredCars);

        _controller.ModelList();

        var filteredCars = unfilteredCars.Where(c => c.Detail.FuelType == filter.SelectedFuelTypes);

        _mockCarService.Setup(s => s.Filtering(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
            It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .Returns(filteredCars);

        // Act
        var result = _controller.ApplyFilters(filter) as PartialViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ModelList/_PartialModelList.cshtml");

        var model = result.Model as FilterViewModel;
        model.Should().NotBeNull();
        model.Cars.Should().BeEquivalentTo(filteredCars);
    }

    [Fact]
    public void ApplyFilters_FiltersBySelectedTransmissionTypes()
    {
        // Arrange
        var unfilteredCars = _carFaker.Generate(5);

        var filter = new FilterViewModel
        {
            SelectedTransmissionTypes = "Automatic"
        };

        _mockCarService.Setup(s => s.GetAllCars()).Returns(unfilteredCars);

        _controller.ModelList();

        var filteredCars = unfilteredCars.Where(c => c.Detail.Transmission == filter.SelectedTransmissionTypes);

        _mockCarService.Setup(s => s.Filtering(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
            It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .Returns(filteredCars);

        // Act
        var result = _controller.ApplyFilters(filter) as PartialViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ModelList/_PartialModelList.cshtml");

        var model = result.Model as FilterViewModel;
        model.Should().NotBeNull();
        model.Cars.Should().BeEquivalentTo(filteredCars);
    }

    [Fact]
    public void ApplyFilters_FiltersBySelectedMakes()
    {
        // Arrange
        var unfilteredCars = _carFaker.Generate(5);

        var filter = new FilterViewModel
        {
            SelectedMakes = "Toyota"
        };

        _mockCarService.Setup(s => s.GetAllCars()).Returns(unfilteredCars);

        _controller.ModelList();

        var filteredCars = unfilteredCars.Where(c => c.Detail.Transmission == filter.SelectedTransmissionTypes);

        _mockCarService.Setup(s => s.Filtering(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
            It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .Returns(filteredCars);

        // Act
        var result = _controller.ApplyFilters(filter) as PartialViewResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/Views/ModelList/_PartialModelList.cshtml");

        var model = result.Model as FilterViewModel;
        model.Should().NotBeNull();
        model.Cars.Should().BeEquivalentTo(filteredCars);
    }
}
