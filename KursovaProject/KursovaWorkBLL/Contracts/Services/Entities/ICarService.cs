using KursovaWork.Domain.Entities.Car;

namespace KursovaWork.Application.Contracts.Services.Entities;

/// <summary>
/// Interface for business logic related to cars
/// </summary>
public interface ICarService
{
    /// <summary>
    /// Method to get car information by its identifier
    /// </summary>
    /// <param name="id">Car identifier</param>
    /// <returns>Car information</returns>
    Car GetCarById(int id);

    /// <summary>
    /// Method to get car information by its make, model, and year of manufacture
    /// </summary>
    /// <param name="make">Car make</param>
    /// <param name="model">Car model</param>
    /// <param name="year">Car year of manufacture</param>
    /// <returns>Car information</returns>
    Car GetCarByInfo(string make, string model, int year);

    /// <summary>
    /// Method to add car information to the database
    /// </summary>
    /// <param name="car">Car information</param>
    void AddCar(Car car);

    /// <summary>
    /// Method to update car information in the database
    /// </summary>
    /// <param name="car">Car information</param>
    void UpdateCar(Car car);

    /// <summary>
    /// Method to delete car information from the database
    /// </summary>
    /// <param name="car">Car information</param>
    void DeleteCar(Car car);

    /// <summary>
    /// Method to get all possible car information
    /// </summary>
    /// <returns>List of all possible car information</returns>
    IEnumerable<Car> GetAllCars();

    /// <summary>
    /// Method to sort the list of models alphabetically.
    /// </summary>
    /// <param name="curList">List of cars</param>
    /// <returns>Sorted list</returns>
    IEnumerable<Car> SortByAlphabet(IEnumerable<Car> curList);

    /// <summary>
    /// Method to sort the list of models by price.
    /// </summary>
    /// <param name="curList">List of cars</param>
    /// <param name="param">Sorting parameter (cheap or expensive).</param>
    /// <returns>Sorted list</returns>
    IEnumerable<Car> SortByPrice(IEnumerable<Car> curList, string param);

    /// <summary>
    /// Method to sort the list of models by novelty (year of manufacture).
    /// </summary>
    /// <param name="curList">List of cars</param>
    /// <returns>Sorted list</returns>
    IEnumerable<Car> SortByNovelty(IEnumerable<Car> curList);

    /// <summary>
    /// Method to filter the list of models
    /// </summary>
    /// <param name="priceFrom">Minimum price.</param>
    /// <param name="priceTo">Maximum price.</param>
    /// <param name="yearFrom">Minimum year of manufacture.</param>
    /// <param name="yearTo">Maximum year of manufacture.</param>
    /// <param name="selectedFuelTypes">List of selected fuel types.</param>
    /// <param name="selectedTransmissionTypes">List of selected transmission types.</param>
    /// <param name="selectedMakes">List of selected car makes.</param>
    /// <returns>Filtered list of models</returns>
    IEnumerable<Car> Filtering(int? priceFrom, int? priceTo, int? yearFrom, int? yearTo, string? selectedFuelTypes, string? selectedTransmissionTypes, string? selectedMakes);
}