using KursovaWork.Application.Contracts.Repositories;
using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Domain.Entities.Car;
using Serilog;

namespace KursovaWork.Infrastructure.Services.Entities;

/// <summary>
/// Implementation of the ICarService interface for business logic related to cars
/// </summary>
public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CarService"/> class.
    /// </summary>
    /// <param name="carRepository">Repository for cars.</param>
    public CarService(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public void AddCar(Car car)
    {
        _carRepository.Add(car);
        Log.Information("Added new car information");
    }

    public void DeleteCar(Car car)
    {
        _carRepository.Delete(car);
        Log.Information("Deleted car information");
    }

    public IEnumerable<Car> Filtering(int? priceFrom, int? priceTo, int? yearFrom, int? yearTo, string? selectedFuelTypes, string? selectedTransmissionTypes, string? selectedMakes)
    {
        Log.Information("Entering car list filtering method");
        var filteredCars = _carRepository.GetAll();
        Log.Information("Retrieving list of all possible cars");

        if (priceFrom.HasValue)
        {
            filteredCars = filteredCars.Where(c => c.Price >= priceFrom.Value).ToList();
            Log.Information("Filtering by price from a specific value");
        }

        if (priceTo.HasValue)
        {
            filteredCars = filteredCars.Where(c => c.Price <= priceTo.Value).ToList();
            Log.Information("Filtering by price up to a specific value");
        }

        if (yearFrom.HasValue)
        {
            filteredCars = filteredCars.Where(c => c.Year >= yearFrom.Value).ToList();
            Log.Information("Filtering by year of manufacture from a specific value");
        }

        if (yearTo.HasValue)
        {
            filteredCars = filteredCars.Where(c => c.Year <= yearTo.Value).ToList();
            Log.Information("Filtering by year of manufacture up to a specific value");
        }

        if (selectedFuelTypes is not null)
        {
            filteredCars = filteredCars.Where(c => selectedFuelTypes.Equals(c.Detail.FuelType)).ToList();
            Log.Information("Filtering by selected fuel type");
        }

        if (selectedTransmissionTypes is not null)
        {
            filteredCars = filteredCars.Where(c => selectedTransmissionTypes.Equals(c.Detail.Transmission)).ToList();
            Log.Information("Filtering by selected transmission type");
        }

        if (selectedMakes is not null)
        {
            filteredCars = filteredCars.Where(c => selectedMakes.Equals(c.Make)).ToList();
            Log.Information("Filtering by selected makes");
        }

        return filteredCars;
    }

    public IEnumerable<Car> GetAllCars()
    {
        Log.Information("Retrieved all possible car information");
        return _carRepository.GetAll();
    }

    public Car GetCarById(int id)
    {
        Log.Information("Retrieved car information by its identifier");
        return _carRepository.GetById(id);
    }

    public Car GetCarByInfo(string make, string model, int year)
    {
        Log.Information("Retrieved car information by its make, model, and year of manufacture");
        return _carRepository.GetByCarInfo(make, model, year);
    }

    public IEnumerable<Car> SortByAlphabet(IEnumerable<Car> curList)
    {
        Log.Information("Entering method to sort the list of models alphabetically");
        return curList.OrderBy(o => o.Make + o.Model);
    }

    public IEnumerable<Car> SortByNovelty(IEnumerable<Car> curList)
    {
        Log.Information("Entering method to sort the list of models by novelty (year of manufacture in descending order)");
        return curList.OrderByDescending(o => o.Year);
    }

    public IEnumerable<Car> SortByPrice(IEnumerable<Car> curList, string param)
    {
        Log.Information("Entering method to sort the list of models by price");

        if (param.Equals("cheap"))
        {
            curList = curList.OrderBy(o => o.Price);
            Log.Information("Sorting by price in ascending order");
        }
        else
        {
            curList = curList.OrderByDescending(o => o.Price);
            Log.Information("Sorting by price in descending order");
        }

        return curList;
    }

    public void UpdateCar(Car car)
    {
        _carRepository.Update(car);
        Log.Information("Updated car information");
    }
}