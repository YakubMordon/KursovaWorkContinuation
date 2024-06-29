using System.ComponentModel.DataAnnotations;

namespace KursovaWork.Domain.Models;

/// <summary>
/// Model for getting email on changing of password.
/// </summary>
public class EmailViewModel
{
    /// <summary>
    /// Email.
    /// </summary>
    [Required(ErrorMessage = "Поле Електронна пошта є обов'язковим")]
    [EmailAddress(ErrorMessage = "Поле, не являється електронною поштою")]
    [Display(Name = "Електронна пошта")]
    public string Email { get; set; }
}