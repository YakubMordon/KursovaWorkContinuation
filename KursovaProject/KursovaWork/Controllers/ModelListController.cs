using Microsoft.AspNetCore.Mvc;
using Serilog;
using KursovaWork.Domain.Entities.Car;
using KursovaWork.Domain.Models;
using KursovaWork.Application.Contracts.Services.Entities;

namespace KursovaWork.UI.Controllers;

/// <summary>
/// Controller responsible for displaying the list of car models.
/// </summary>
public class ModelListController : Controller
{
    private readonly ICarService _carService;
    private static List<Car> _curList;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModelListController"/> class.
    /// </summary>
    /// <param name="carService">The service interface for performing car-related actions.</param>
    public ModelListController(ICarService carService)
    {
        _carService = carService;
        _curList = new List<Car>();
    }

    /// <summary>
    /// Retrieves the page displaying the list of car models.
    /// </summary>
    /// <returns>The page displaying the list of car models.</returns>
    public IActionResult ModelList()
    {
        Log.Information("Entering method to navigate to the model list page");
        _curList = _carService.GetAllCars().ToList();

        Log.Information("Fetching all possible car models");

        Log.Information("Setting all car models list as current");
        var model = new FilterViewModel();
        FilterViewModel.OrigCars = _curList;
        model.Cars = _curList;

        Log.Information("Navigating to the model list page");
        return View("~/Views/ModelList/ModelList.cshtml", model);
    }

    /// <summary>
    /// Sorts the list of models alphabetically.
    /// </summary>
    /// <returns>The page displaying the list of car models with the sorted list.</returns>
    public IActionResult SortByAlphabet()
    {
        Log.Information("Entering method to sort the model list alphabetically");
        _curList = _carService.SortByAlphabet(_curList).ToList();

        Log.Information("Setting sorted list as current");
        var model = new FilterViewModel();
        model.Cars = _curList;

        Log.Information("Navigating to the model list page");

        return PartialView("~/Views/ModelList/_PartialModelList.cshtml", model);
    }

    /// <summary>
    /// Sorts the list of models by price.
    /// </summary>
    /// <param name="param1">Sorting parameter (cheap or expensive).</param>
    /// <returns>The page displaying the list of car models with the sorted list.</returns>
    public IActionResult SortByPrice(string param1)
    {
        _curList = _carService.SortByPrice(_curList, param1).ToList();

        Log.Information("Setting sorted list as current");

        var model = new FilterViewModel();
        model.Cars = _curList;

        Log.Information("Navigating to the model list page");

        return PartialView("~/Views/ModelList/_PartialModelList.cshtml", model);
    }

    /// <summary>
    /// Sorts the list of models by novelty (year of manufacture).
    /// </summary>
    /// <returns>The page displaying the list of car models with the sorted list.</returns>
    public IActionResult SortByNovelty()
    {
        Log.Information("Entering method to sort the model list by descending year (Novelty)");

        _curList = _carService.SortByNovelty(_curList).ToList();

        Log.Information("Setting sorted list as current");

        var model = new FilterViewModel();
        model.Cars = _curList;

        Log.Information("Navigating to the model list page");

        return PartialView("~/Views/ModelList/_PartialModelList.cshtml", model);
    }

    /// <summary>
    /// Applies filters to the list of car models.
    /// </summary>
    /// <param name="filter">The model containing user-entered filters.</param>
    /// <returns>The page displaying the list of car models with the filtered list.</returns>
    public IActionResult ApplyFilters(FilterViewModel filter)
    {
        _curList = _carService.Filtering(filter.PriceFrom, filter.PriceTo,
                filter.YearFrom, filter.YearTo,
                filter.SelectedFuelTypes,
                filter.SelectedTransmissionTypes,
                filter.SelectedMakes)
            .ToList();

        Log.Information("Setting filtered list as current");

        filter.Cars = _curList;

        Log.Information("Navigating to the model list page");

        return PartialView("~/Views/ModelList/_PartialModelList.cshtml", filter);
    }
}