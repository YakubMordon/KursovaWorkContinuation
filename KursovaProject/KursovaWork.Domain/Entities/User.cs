using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWork.Domain.Entities;

/// <summary>
/// Represents a user.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The user's first name.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    /// <summary>
    /// The user's last name.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    /// <summary>
    /// The user's email address.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// The user's password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }

    /// <summary>
    /// The confirmation of the user's password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    /// <summary>
    /// The date and time the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// A collection of orders associated with the user.
    /// </summary>
    public virtual ICollection<Order> Orders { get; set; }

    /// <summary>
    /// The user's credit card (one-to-one relationship).
    /// </summary>
    [ForeignKey("UserId")]
    public virtual Card CreditCard { get; set; }
}