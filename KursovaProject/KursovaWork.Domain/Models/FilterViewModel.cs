using KursovaWork.Domain.Entities.Car;

namespace KursovaWork.Domain.Models;

/// <summary>
/// Model for filtering cars.
/// </summary>
public class FilterViewModel
{
    /// <summary>
    /// Minimum price threshold.
    /// </summary>
    public int? PriceFrom { get; set; }

    /// <summary>
    /// Maximum price threshold.
    /// </summary>
    public int? PriceTo { get; set; }

    /// <summary>
    /// Minimum manufacturing year.
    /// </summary>
    public int? YearFrom { get; set; }

    /// <summary>
    /// Maximum manufacturing year.
    /// </summary>
    public int? YearTo { get; set; }

    /// <summary>
    /// List of selected fuel types.
    /// </summary>
    public string? SelectedFuelTypes { get; set; }

    /// <summary>
    /// List of selected transmission types.
    /// </summary>
    public string? SelectedTransmissionTypes { get; set; }

    /// <summary>
    /// List of selected car makes.
    /// </summary>
    public string? SelectedMakes { get; set; }

    /// <summary>
    /// List of current car models.
    /// </summary>
    public List<Car> Cars { get; set; }

    /// <summary>
    /// Initial list of car models.
    /// </summary>
    public static List<Car> OrigCars { get; set; }
}