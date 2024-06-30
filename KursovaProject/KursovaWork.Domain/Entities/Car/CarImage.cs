using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWork.Domain.Entities.Car;

/// <summary>
/// Represents a car image.
/// </summary>
[Table("car_image")]
public class CarImage : BaseEntity
{
    /// <summary>
    /// The identifier for the car image.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// The identifier for the car to which the image belongs.
    /// </summary>
    [Required]
    [ForeignKey(nameof(Car))]
    [Column("car_id")]
    public int CarId { get; set; }

    /// <summary>
    /// The URL of the car image.
    /// </summary>
    [Required]
    [Column("url")]
    public string Url { get; set; }

    /// <summary>
    /// The car object to which the image belongs.
    /// </summary>
    public virtual Car Car { get; set; }
}