using System.ComponentModel.DataAnnotations;

namespace KursovaWork.Domain.Models;

/// <summary>
/// Model for getting signup data.
/// </summary>
public class SignUpViewModel
{
    /// <summary>
    /// First Name.
    /// </summary>
    [Required(ErrorMessage = "Поле Ім\'я є обов\'язковим")]
    [StringLength(50)]
    [Display(Name = "Ім'я")]
    public string FirstName { get; set; }

    /// <summary>
    /// Last Name.
    /// </summary>
    [Required(ErrorMessage = "Поле Прізвище є обов'язковим")]
    [StringLength(50)]
    [Display(Name = "Прізвище")]
    public string LastName { get; set; }

    /// <summary>
    /// Email.
    /// </summary>
    [Required(ErrorMessage = "Поле Електронна пошта є обов'язковим")]
    [EmailAddress(ErrorMessage = "Поле, не являється електронною поштою")]
    [Display(Name = "Електронна пошта")]
    public string Email { get; set; }

    /// <summary>
    /// Password.
    /// </summary>
    [Required(ErrorMessage = "Поле Пароль є обов'язковим")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль повинен містити мінімум 6 символів")]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    /// <summary>
    /// Password Confirmation.
    /// </summary>
    [Required(ErrorMessage = "Поле Підтвердження пароля є обов'язковим")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Поле Підтвердження паролю та Пароль повинні бути одинакові")]
    [Display(Name = "Підтвердження пароля")]
    public string ConfirmPassword { get; set; }
}