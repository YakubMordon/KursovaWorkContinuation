using System.ComponentModel.DataAnnotations;

namespace KursovaWorkDAL.Entity.Entities.Car;

/// <summary>
/// Car details.
/// </summary>
public class CarDetail : BaseEntity
{
    /// <summary>
    /// The identifier for the car detail.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The identifier for the car.
    /// </summary>
    [Required]
    public int CarId { get; set; }

    /// <summary>
    /// The color of the car.
    /// </summary>
    [StringLength(50)]
    public string Color { get; set; }

    /// <summary>
    /// The type of transmission of the car.
    /// </summary>
    [StringLength(50)]
    public string Transmission { get; set; }

    /// <summary>
    /// The type of fuel of the car.
    /// </summary>
    [StringLength(50)]
    public string FuelType { get; set; }

    /// <summary>
    /// The car to which the details belong.
    /// </summary>
    public virtual CarInfo Car { get; set; }
}