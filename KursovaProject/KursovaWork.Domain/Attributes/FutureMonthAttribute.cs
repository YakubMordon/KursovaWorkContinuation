using System.ComponentModel.DataAnnotations;

namespace KursovaWork.Domain.Attributes;

/// <summary>
/// Validation attribute to check if the date is in the future or current.
/// </summary>
public class FutureMonthAttribute : ValidationAttribute
{
    /// <summary>
    /// Method to validate if the date is in the future or current.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="validationContext">The context in which validation is performed.</param>
    /// <returns>Success or failure of the validation.</returns>
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not int month || !IsValidYear(validationContext, out var year) || !IsValidExpiration(month, year))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }

    /// <summary>
    /// Method to validate if the year is valid.
    /// </summary>
    /// <param name="validationContext">The context in which validation is performed.</param>
    /// <param name="year">The year.</param>
    /// <returns>Success or failure of the validation.</returns>
    private bool IsValidYear(ValidationContext validationContext, out int year)
    {
        year = 0;
        var yearProperty = validationContext.ObjectType.GetProperty("ExpirationYear");
        var yearValue = yearProperty?.GetValue(validationContext.ObjectInstance);
        return yearValue is int yearInt && yearInt >= DateTime.Now.Year % 2000;
    }

    /// <summary>
    /// Method to validate if the expiration date is in the future or current.
    /// </summary>
    /// <param name="month">The month.</param>
    /// <param name="year">The year.</param>
    /// <returns>Success or failure of the validation.</returns>
    private bool IsValidExpiration(int month, int year)
    {
        var currentYear = DateTime.Now.Year % 2000;
        var currentMonth = DateTime.Now.Month;
        return year > currentYear || year == currentYear && month >= currentMonth;
    }
}