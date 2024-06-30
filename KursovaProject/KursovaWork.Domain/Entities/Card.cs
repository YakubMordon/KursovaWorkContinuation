using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWork.Domain.Entities;

/// <summary>
/// Represents a credit card entity.
/// </summary>
[Table("card")]
public class Card : BaseEntity
{
    /// <summary>
    /// User identifier.
    /// </summary>
    [Key]
    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// Credit card number.
    /// </summary>
    [Required]
    [Column("card_number")]
    public string CardNumber { get; set; }

    /// <summary>
    /// Credit card holder's name.
    /// </summary>
    [Required]
    [Column("cardholder_name")]
    public string CardHolderName { get; set; }

    /// <summary>
    /// Credit card expiration month.
    /// </summary>
    [Required]
    [Column("expiration_month")]
    public string ExpirationMonth { get; set; }

    /// <summary>
    /// Gets or sets the credit card expiration year.
    /// </summary>
    [Required]
    [Column("expiration_year")]
    public string ExpirationYear { get; set; }

    /// <summary>
    /// Credit card CVV code.
    /// </summary>
    [Required]
    [Column("cvv")]
    public string Cvv { get; set; }

    /// <summary>
    /// Relationship with the user.
    /// </summary>
    public virtual User User { get; set; }
}