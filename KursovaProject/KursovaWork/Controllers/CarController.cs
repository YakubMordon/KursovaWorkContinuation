using Microsoft.AspNetCore.Mvc;
using KursovaWorkBLL.Contracts;

namespace KursovaWork.Controllers
{
    /// <summary>
    /// Controller responsible for actions related to cars.
    /// </summary>
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ILogger<CarController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarController"/> class.
        /// </summary>
        /// <param name="carService">The service for car operations.</param>
        /// <param name="logger">The logger for logging.</param>
        public CarController(ICarService carService, ILogger<CarController> logger)
        {
            _carService = carService;
            _logger = logger;
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
            _logger.LogInformation("Entering Car method");

            if (!int.TryParse(param3, out int year))
            {
                _logger.LogError("Invalid year parameter");
                return View("Error");
            }

            var car = _carService.GetCarByInfo(param1, param2, year);

            if (car is not null)
            {
                _logger.LogInformation("Car found, navigating to car page");
                return View(car);
            }

            _logger.LogError("Car not found");
            return View("Error");
        }
    }
}
