using Microsoft.AspNetCore.Mvc;
using Serilog;
using KursovaWork.Domain.Entities;
using KursovaWork.Application.Contracts.Services.Entities;

namespace KursovaWork.UI.Controllers;

/// <summary>
/// Controller responsible for handling car configuration actions.
/// </summary>
public class ConfiguratorController : Controller
{
    private readonly ICarService _carService;
    public static ConfiguratorOptions? Options { get; set; }
    private static string[] _param = new string[3];

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfiguratorController"/> class.
    /// </summary>
    /// <param name="carService">The service for car operations.</param>
    public ConfiguratorController(ICarService carService)
    {
        _carService = carService;
    }

    /// <summary>
    /// Retrieves the page for car configuration.
    /// </summary>
    /// <param name="param1">Parameter 1 (car make).</param>
    /// <param name="param2">Parameter 2 (car model).</param>
    /// <param name="param3">Parameter 3 (car production year).</param>
    /// <returns>The page with car configurator or error page.</returns>
    public IActionResult Configurator(string param1, string param2, string param3)
    {
        Log.Information("Entering car configurator transition function");

        _param = new[] { param1, param2, param3 };

        var year = int.Parse(param3);

        var car = _carService.GetCarByInfo(param1, param2, year);

        Log.Information("Car found, transitioning to car configurator page");
        return View(car);
    }

    /// <summary>
    /// Processes the selected car configuration parameters.
    /// </summary>
    /// <param name="color">Car color.</param>
    /// <param name="transmission">Car transmission type.</param>
    /// <param name="fuelType">Car fuel type.</param>
    /// <returns>The car payment page or the car configurator page with error messages.</returns>
    public IActionResult Submit(string color, string transmission, string fuelType)
    {
        Log.Information("Entering car configuration confirmation function");

        var errors = new Dictionary<string, string>();

        if (string.IsNullOrEmpty(color))
        {
            errors["color"] = "Select a color";
            Log.Information("Color was not selected");
        }

        if (string.IsNullOrEmpty(transmission))
        {
            errors["transmission"] = "Select a transmission type";
            Log.Information("Transmission type was not selected");
        }

        if (string.IsNullOrEmpty(fuelType))
        {
            errors["fuelType"] = "Select a fuel type";
            Log.Information("Fuel type was not selected");
        }

        if (errors.Count > 0)
        {
            Log.Information("One or more parameters were not selected in the configurator");
            return Json(new { errors });
        }

        Options = new ConfiguratorOptions()
        {
            Color = color,
            Transmission = transmission,
            FuelType = fuelType
        };

        Log.Information("Transitioning to car payment page");
        return Json(new { redirect = Url.Action("Payment", "Payment", new { param1 = _param[0], param2 = _param[1], param3 = _param[2] }) });
    }
}