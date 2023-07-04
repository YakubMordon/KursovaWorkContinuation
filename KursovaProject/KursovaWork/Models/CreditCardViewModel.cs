using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using KursovaWorkDAL.Entity.Entities;

namespace KursovaWork.Models
{
    /// <summary>
    /// Клас для получення даних про кредитну карту.
    /// </summary>
    public class CreditCardViewModel
    {
        /// <summary>
        /// Ідентифікатор користувача
        /// </summary>
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        
        /// <summary>
        /// Номер кредитної карти
        /// </summary>
        [Required(ErrorMessage = "Поле Номер карти є обов'язковим")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Довжина номеру карти повинна бути мінімум 16")]
        [RegularExpression(@"^(?:\d{16})$", ErrorMessage = "Неправильний номер карти")]
        public string CardNumber { get; set; }

        /// <summary>
        /// Ім'я та прізвище власника карти
        /// </summary>
        [Required(ErrorMessage = "Поле Ім'я та прізвище є обов'язковим")]
        public string CardHolderName { get; set; }

        /// <summary>
        /// Термін дії(місяць)
        /// </summary>
        [Required(ErrorMessage = "Поле Термін дії(місяць) є обов'язковим")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Довжина Терміну дії(місяць) повинна бути мінімум 2")]
        [Range(1,12, ErrorMessage = "Введений місяць не є корректним")]
        [FutureMonth(ErrorMessage = "Дата повинна бути або такою ж або більшою")]
        public string ExpirationMonth { get; set; }

        /// <summary>
        /// Термін дії(рік)
        /// </summary>
        [Required(ErrorMessage = "Поле Термін дії(рік) є обов'язковим")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Довжина Терміну дії(рік) повинна бути мінімум 2")]
        [FutureYear(ErrorMessage = "Рік повинен бути більшим або дорівнювати теперешньому")]
        public string ExpirationYear { get; set; }

        /// <summary>
        /// CVV код
        /// </summary>
        [Required(ErrorMessage = "Поле CVV є обов'язковим")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Довжина CVV повинна бути мінімум 3")]
        [RegularExpression(@"^(?:\d{3})$", ErrorMessage = "Неправильний номер CVV")]
        public string CVV { get; set; }

        /// <summary>
        /// Метод перетворення об'єкту класу CreditCardViewModel у об'єкт класу Card
        /// </summary>
        /// <returns>Перетворений об'єкт класу Card</returns>
        public Card ToCard()
        {
            return new Card
            {
                CardNumber = CardNumber,
                CardHolderName = CardHolderName,
                ExpirationMonth = ExpirationMonth,
                ExpirationYear = ExpirationYear,
                CVV = CVV
            };
        }
    }

    /// <summary>
    /// Валідаційний атрибут, що перевіряє, чи є рік у майбутньому або теперешньому.
    /// </summary>
    public class FutureYearAttribute : ValidationAttribute
    {
        /// <summary>
        /// Метод перевірки чи є рік у майбутньому або теперешньому
        /// </summary>
        /// <param name="value">Параметр, який перевіряється на валідність</param>
        /// <param name="validationContext">Описує контекст у якому валідація виконується</param>
        /// <returns>Успіх або провал валідації</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return 
                value is null || !int.TryParse(value.ToString(), out int year) || year < DateTime.Now.Year % 2000
                ? new ValidationResult(ErrorMessage)
                : ValidationResult.Success;
        }
    }

    /// <summary>
    /// Валідаційний атрибут, що перевіряє, чи є дата у майбутньому або теперешньому.
    /// </summary>
    public class FutureMonthAttribute : ValidationAttribute
    {
        /// <summary>
        /// Метод перевірки чи є дата у майбутньому або теперешньому
        /// </summary>
        /// <param name="value">Параметр, який перевіряється на валідність</param>
        /// <param name="validationContext">Описує контекст у якому валідація виконується</param>
        /// <returns>Успіх або провал валідації</returns>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not int month || !IsValidYear(validationContext, out int year) || !IsValidExpiration(month, year))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Метод перевірки чи рік є валідним
        /// </summary>
        /// <param name="validationContext">Описує контекст у якому валідація виконується</param>
        /// <param name="year">Рік</param>
        /// <returns>Успіх або провал валідації</returns>
        private bool IsValidYear(ValidationContext validationContext, out int year)
        {
            year = 0;
            var yearProperty = validationContext.ObjectType.GetProperty("ExpirationYear");
            var yearValue = yearProperty?.GetValue(validationContext.ObjectInstance);
            return yearValue is int yearInt && yearInt >= DateTime.Now.Year % 2000;
        }

        /// <summary>
        /// Метод перевірки чи дата є у майбутньому або теперешньому
        /// </summary>
        /// <param name="month">Місяць</param>
        /// <param name="year">Рік</param>
        /// <returns>Успіх або провал валідації</returns>
        private bool IsValidExpiration(int month, int year)
        {
            var currentYear = DateTime.Now.Year % 2000;
            var currentMonth = DateTime.Now.Month;
            return year > currentYear || (year == currentYear && month >= currentMonth);
        }


    }

}

