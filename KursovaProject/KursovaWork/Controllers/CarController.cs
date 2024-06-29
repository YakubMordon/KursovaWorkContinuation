using Microsoft.AspNetCore.Mvc;
using Serilog;
using KursovaWork.Application.Contracts.Services.Entities;

namespace KursovaWork.UI.Controllers;

/// <summary>
/// Controller responsible for actions related to cars.
/// </summary>
public class CarController : Controller
{
    private readonly ICarService _carService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CarController"/> class.
    /// </summary>
    /// <param name="carService">The service for car operations.</param>
    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    /// <summary>
    /// Retrieves the page with detailed information about a car.
    /// </summary>
    /// <param name="param1">Parameter 1 (car make).</param>
    /// <param name="param2">Parameter 2 (car model).</param>
    /// <param name="param3">Parameter 3 (car year).</param>
    /// <returns>The page with detailed car information or an error page.</returns>
    public IActionResult Car(string param1, string param2, string param3)
    {
        Log.Information("Entering Car method");

        if (!int.TryParse(param3, out var year))
        {
            Log.Error("Invalid year parameter");
            return View("Error");
        }

        var car = _carService.GetCarByInfo(param1, param2, year);

        if (car is not null)
        {
            Log.Information("Car found, navigating to car page");
            return View(car);
        }

        Log.Error("Car not found");
        return View("Error");
    }
}