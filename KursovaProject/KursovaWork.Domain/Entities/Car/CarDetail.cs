using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWork.Domain.Entities.Car;

/// <summary>
/// Car details.
/// </summary>
[Table("car_detail")]
public class CarDetail : BaseEntity
{
    /// <summary>
    /// The identifier for the car detail.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// The identifier for the car.
    /// </summary>
    [Required]
    [ForeignKey(nameof(Car))]
    [Column("car_id")]
    public int CarId { get; set; }

    /// <summary>
    /// The color of the car.
    /// </summary>
    [StringLength(50)]
    [Column("color")]
    public string Color { get; set; }

    /// <summary>
    /// The type of transmission of the car.
    /// </summary>
    [StringLength(50)]
    [Column("transmission")]
    public string Transmission { get; set; }

    /// <summary>
    /// The type of fuel of the car.
    /// </summary>
    [StringLength(50)]
    [Column("fuel_type")]
    public string FuelType { get; set; }

    /// <summary>
    /// The car to which the details belong.
    /// </summary>
    public virtual Car Car { get; set; }
}