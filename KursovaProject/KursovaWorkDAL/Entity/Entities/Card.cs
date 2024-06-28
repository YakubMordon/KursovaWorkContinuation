using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWorkDAL.Entity.Entities;

/// <summary>
/// Represents a credit card entity.
/// </summary>
public class Card : BaseEntity
{
    /// <summary>
    /// User identifier.
    /// </summary>
    [Key]
    [ForeignKey("User")]
    public int UserId { get; set; }

    /// <summary>
    /// Credit card number.
    /// </summary>
    [Required]
    public string CardNumber { get; set; }

    /// <summary>
    /// Credit card holder's name.
    /// </summary>
    [Required]
    public string CardHolderName { get; set; }

    /// <summary>
    /// Credit card expiration month.
    /// </summary>
    [Required]
    public string ExpirationMonth { get; set; }

    /// <summary>
    /// Gets or sets the credit card expiration year.
    /// </summary>
    [Required]
    public string ExpirationYear { get; set; }

    /// <summary>
    /// Credit card CVV code.
    /// </summary>
    [Required]
    public string Cvv { get; set; }

    /// <summary>
    /// Relationship with the user.
    /// </summary>
    public virtual User User { get; set; }
}