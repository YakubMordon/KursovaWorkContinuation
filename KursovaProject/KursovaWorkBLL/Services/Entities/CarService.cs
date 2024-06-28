using KursovaWorkBLL.Contracts;
using KursovaWorkDAL.Entity.Entities.Car;
using KursovaWorkDAL.Repositories.Contracts;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace KursovaWorkBLL.Services.Entities
{
    /// <summary>
    /// Implementation of the ICarService interface for business logic related to cars
    /// </summary>
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly ILogger<CarService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarService"/> class.
        /// </summary>
        /// <param name="carRepository">Repository for cars.</param>
        /// <param name="logger">Logger object for logging events.</param>
        public CarService(ICarRepository carRepository, ILogger<CarService> logger)
        {
            _carRepository = carRepository;
            _logger = logger;
        }

        public void AddCar(CarInfo car)
        {
            _carRepository.Add(car);
            _logger.LogInformation("Added new car information");
        }

        public void DeleteCar(CarInfo car)
        {
            _carRepository.Delete(car);
            _logger.LogInformation("Deleted car information");
        }

        public IEnumerable<CarInfo> Filtering(int? PriceFrom, int? PriceTo, int? YearFrom, int? YearTo, string? SelectedFuelTypes, string? SelectedTransmissionTypes, string? SelectedMakes)
        {
            _logger.LogInformation("Entering car list filtering method");
            var filteredCars = _carRepository.GetAll();
            _logger.LogInformation("Retrieving list of all possible cars");

            if (PriceFrom.HasValue)
            {
                filteredCars = filteredCars.Where(c => c.Price >= PriceFrom.Value).ToList();
                _logger.LogInformation("Filtering by price from a specific value");
            }

            if (PriceTo.HasValue)
            {
                filteredCars = filteredCars.Where(c => c.Price <= PriceTo.Value).ToList();
                _logger.LogInformation("Filtering by price up to a specific value");
            }

            if (YearFrom.HasValue)
            {
                filteredCars = filteredCars.Where(c => c.Year >= YearFrom.Value).ToList();
                _logger.LogInformation("Filtering by year of manufacture from a specific value");
            }

            if (YearTo.HasValue)
            {
                filteredCars = filteredCars.Where(c => c.Year <= YearTo.Value).ToList();
                _logger.LogInformation("Filtering by year of manufacture up to a specific value");
            }

            if (SelectedFuelTypes != null)
            {
                filteredCars = filteredCars.Where(c => SelectedFuelTypes.Equals(c.Detail.FuelType)).ToList();
                _logger.LogInformation("Filtering by selected fuel type");
            }

            if (SelectedTransmissionTypes != null)
            {
                filteredCars = filteredCars.Where(c => SelectedTransmissionTypes.Equals(c.Detail.Transmission)).ToList();
                _logger.LogInformation("Filtering by selected transmission type");
            }

            if (SelectedMakes != null)
            {
                filteredCars = filteredCars.Where(c => SelectedMakes.Equals(c.Make)).ToList();
                _logger.LogInformation("Filtering by selected makes");
            }

            return filteredCars;
        }

        public IEnumerable<CarInfo> GetAllCars()
        {
            _logger.LogInformation("Retrieved all possible car information");
            return _carRepository.GetAll();
        }

        public CarInfo GetCarById(int id)
        {
            _logger.LogInformation("Retrieved car information by its identifier");
            return _carRepository.GetById(id);
        }

        public CarInfo GetCarByInfo(string make, string model, int year)
        {
            _logger.LogInformation("Retrieved car information by its make, model, and year of manufacture");
            return _carRepository.GetByCarInfo(make, model, year);
        }

        public IEnumerable<CarInfo> SortByAlphabet(IEnumerable<CarInfo> _curList)
        {
            _logger.LogInformation("Entering method to sort the list of models alphabetically");
            return _curList.OrderBy(o => o.Make + o.Model);
        }

        public IEnumerable<CarInfo> SortByNovelty(IEnumerable<CarInfo> _curList)
        {
            _logger.LogInformation("Entering method to sort the list of models by novelty (year of manufacture in descending order)");
            return _curList.OrderByDescending(o => o.Year);
        }

        public IEnumerable<CarInfo> SortByPrice(IEnumerable<CarInfo> _curList, string param)
        {
            _logger.LogInformation("Entering method to sort the list of models by price");

            if (param.Equals("cheap"))
            {
                _curList = _curList.OrderBy(o => o.Price);
                _logger.LogInformation("Sorting by price in ascending order");
            }
            else
            {
                _curList = _curList.OrderByDescending(o => o.Price);
                _logger.LogInformation("Sorting by price in descending order");
            }

            return _curList;
        }

        public void UpdateCar(CarInfo car)
        {
            _carRepository.Update(car);
            _logger.LogInformation("Updated car information");
        }
    }
}
