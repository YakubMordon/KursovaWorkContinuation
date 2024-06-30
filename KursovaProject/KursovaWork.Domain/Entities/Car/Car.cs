using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWork.Domain.Entities.Car;

/// <summary>
/// Class representing car information.
/// </summary>
[Table("cars")]
public class Car : BaseEntity
{
    /// <summary>
    /// Unique identifier for the car.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Car make.
    /// </summary>
    [Required]
    [StringLength(50)]
    [Column("make")]
    public string Make { get; set; }

    /// <summary>
    /// Car model.
    /// </summary>
    [Required]
    [StringLength(50)]
    [Column("model")]
    public string Model { get; set; }

    /// <summary>
    /// Year of manufacture of the car.
    /// </summary>
    [Required]
    [Range(1900, 2100)]
    [Column("year")]
    public int Year { get; set; }

    /// <summary>
    /// Price of the car.
    /// </summary>
    [Required]
    [Range(0, double.MaxValue)]
    [Column("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Description of the car.
    /// </summary>
    [StringLength(500)]
    [Column("description")]
    public string Description { get; set; }

    /// <summary>
    /// Car images.
    /// </summary>
    public virtual ICollection<CarImage> Images { get; set; }

    /// <summary>
    /// Car details.
    /// </summary>
    public virtual CarDetail Detail { get; set; }

    /// <summary>
    /// Car orders.
    /// </summary>
    public virtual ICollection<Order> Orders { get; set; }
}