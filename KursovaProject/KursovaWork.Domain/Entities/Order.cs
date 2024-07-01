using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWork.Domain.Entities;

/// <summary>
/// Represents an order.
/// </summary>
[Table("order")]
public class Order : BaseEntity
{
    /// <summary>
    /// Order identifier.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Car identifier.
    /// </summary>
    [Required]
    [ForeignKey(nameof(Car))]
    [Column("car_id")]
    public int CarId { get; set; }

    /// <summary>
    /// User identifier.
    /// </summary>
    [Required]
    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// Order price.
    /// </summary>
    [Required]
    [Range(0, double.MaxValue)]
    [Column("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Order date.
    /// </summary>
    [Required]
    [Column("order_date")]
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// The car associated with the order.
    /// </summary>
    public virtual Car.Car Car { get; set; }

    /// <summary>
    /// The user associated with the order.
    /// </summary>
    public virtual User User { get; set; }

    /// <summary>
    /// Configurator options associated with the order.
    /// </summary>
    public virtual ConfiguratorOptions? ConfiguratorOptions { get; set; }
}