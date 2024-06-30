using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWork.Domain.Entities;

/// <summary>
/// Class representing the configurator options.
/// </summary>
[Table("configuration_options")]
public class ConfiguratorOptions : BaseEntity
{
    /// <summary>
    /// Configurator options identifier.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// The identifier of the order to which the configurator options belong.
    /// </summary>
    [Required]
    [ForeignKey(nameof(Order))]
    [Column("order_id")]
    public int OrderId { get; set; }

    /// <summary>
    /// The color of the car.
    /// </summary>
    [StringLength(50)]
    [Column("color")]
    public string? Color { get; set; }

    /// <summary>
    /// The transmission type of the car.
    /// </summary>
    [StringLength(50)]
    [Column("transmission")]
    public string? Transmission { get; set; }

    /// <summary>
    /// The fuel type of the car.
    /// </summary>
    [StringLength(50)]
    [Column("fuel_type")]
    public string? FuelType { get; set; }

    /// <summary>
    /// Relationship with the order.
    /// </summary>
    public virtual Order? Order { get; set; }
}