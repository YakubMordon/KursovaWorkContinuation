using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using KursovaWork.Domain.Attributes;

namespace KursovaWork.Domain.Models;

/// <summary>
/// Model for getting Credit Card data.
/// </summary>
public class CreditCardViewModel
{
    /// <summary>
    /// User identifier.
    /// </summary>
    [Key]
    [ForeignKey("User")]
    public int UserId { get; set; }

    /// <summary>
    /// Card Number.
    /// </summary>
    [Required(ErrorMessage = "Поле Номер карти є обов'язковим")]
    [StringLength(16, MinimumLength = 16, ErrorMessage = "Довжина номеру карти повинна бути мінімум 16")]
    [RegularExpression(@"^(?:\d{16})$", ErrorMessage = "Неправильний номер карти")]
    public string CardNumber { get; set; }

    /// <summary>
    /// Full name of card holder.
    /// </summary>
    [Required(ErrorMessage = "Поле Ім'я та прізвище є обов'язковим")]
    public string CardHolderName { get; set; }

    /// <summary>
    /// Expiration Month.
    /// </summary>
    [Required(ErrorMessage = "Поле Термін дії(місяць) є обов'язковим")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Довжина Терміну дії(місяць) повинна бути мінімум 2")]
    [Range(1, 12, ErrorMessage = "Введений місяць не є корректним")]
    [FutureMonth(ErrorMessage = "Дата повинна бути або такою ж або більшою")]
    public string ExpirationMonth { get; set; }

    /// <summary>
    /// Expiration Year.
    /// </summary>
    [Required(ErrorMessage = "Поле Термін дії(рік) є обов'язковим")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Довжина Терміну дії(рік) повинна бути мінімум 2")]
    [FutureYear(ErrorMessage = "Рік повинен бути більшим або дорівнювати теперешньому")]
    public string ExpirationYear { get; set; }

    /// <summary>
    /// CVV code.
    /// </summary>
    [Required(ErrorMessage = "Поле CVV є обов'язковим")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Довжина CVV повинна бути мінімум 3")]
    [RegularExpression(@"^(?:\d{3})$", ErrorMessage = "Неправильний номер CVV")]
    public string Cvv { get; set; }
}