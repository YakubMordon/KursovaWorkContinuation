using KursovaWorkDAL.Entity.Entities.Car;
using KursovaWork.Models;
using Microsoft.AspNetCore.Mvc;
using KursovaWorkBLL.Contracts;

namespace KursovaWork.Controllers
{
    /// <summary>
    /// Controller responsible for displaying the list of car models.
    /// </summary>
    public class ModelListController : Controller
    {
        private readonly ICarService _carService;
        private readonly ILogger<ModelListController> _logger;
        private static List<CarInfo> _curList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelListController"/> class.
        /// </summary>
        /// <param name="carService">The service interface for performing car-related actions.</param>
        /// <param name="logger">ILogger for logging events.</param>
        public ModelListController(ICarService carService, ILogger<ModelListController> logger)
        {
            _carService = carService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the page displaying the list of car models.
        /// </summary>
        /// <returns>The page displaying the list of car models.</returns>
        public IActionResult ModelList()
        {
            _logger.LogInformation("Entering method to navigate to the model list page");
            _curList = _carService.GetAllCars().ToList();

            _logger.LogInformation("Fetching all possible car models");

            _logger.LogInformation("Setting all car models list as current");
            var model = new FilterViewModel();
            FilterViewModel.origCars = _curList;
            model.cars = _curList;

            _logger.LogInformation("Navigating to the model list page");
            return View(model);
        }

        /// <summary>
        /// Sorts the list of models alphabetically.
        /// </summary>
        /// <returns>The page displaying the list of car models with the sorted list.</returns>
        public IActionResult SortByAlphabet()
        {
            _logger.LogInformation("Entering method to sort the model list alphabetically");
            _curList = _carService.SortByAlphabet(_curList).ToList();

            _logger.LogInformation("Setting sorted list as current");
            var model = new FilterViewModel();
            model.cars = _curList;

            _logger.LogInformation("Navigating to the model list page");

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

            _logger.LogInformation("Setting sorted list as current");

            var model = new FilterViewModel();
            model.cars = _curList;

            _logger.LogInformation("Navigating to the model list page");

            return PartialView("~/Views/ModelList/_PartialModelList.cshtml", model);
        }

        /// <summary>
        /// Sorts the list of models by novelty (year of manufacture).
        /// </summary>
        /// <returns>The page displaying the list of car models with the sorted list.</returns>
        public IActionResult SortByNovelty()
        {
            _logger.LogInformation("Entering method to sort the model list by descending year (Novelty)");

            _curList = _carService.SortByNovelty(_curList).ToList();

            _logger.LogInformation("Setting sorted list as current");

            var model = new FilterViewModel();
            model.cars = _curList;

            _logger.LogInformation("Navigating to the model list page");

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

            _logger.LogInformation("Setting filtered list as current");

            filter.cars = _curList;

            _logger.LogInformation("Navigating to the model list page");

            return PartialView("~/Views/ModelList/_PartialModelList.cshtml", filter);
        }
    }
}
