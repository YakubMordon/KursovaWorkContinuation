using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWork.Domain.Entities;

/// <summary>
/// Represents a user.
/// </summary>
[Table("user")]
public class User : BaseEntity
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// The user's first name.
    /// </summary>
    [Required]
    [StringLength(50)]
    [Column("first_name")]
    public string FirstName { get; set; }

    /// <summary>
    /// The user's last name.
    /// </summary>
    [Required]
    [StringLength(50)]
    [Column("last_name")]
    public string LastName { get; set; }

    /// <summary>
    /// The user's email address.
    /// </summary>
    [Required]
    [EmailAddress]
    [Column("email")]
    public string Email { get; set; }

    /// <summary>
    /// The user's password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    [Column("password")]
    public string Password { get; set; }

    /// <summary>
    /// The confirmation of the user's password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [Column("confirm_password")]
    public string ConfirmPassword { get; set; }

    /// <summary>
    /// The date and time the user was created.
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// A collection of orders associated with the user.
    /// </summary>
    public virtual ICollection<Order> Orders { get; set; }

    /// <summary>
    /// The user's credit card (one-to-one relationship).
    /// </summary>
    public virtual Card CreditCard { get; set; }
}