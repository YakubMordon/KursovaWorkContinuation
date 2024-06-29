using System.ComponentModel.DataAnnotations;

namespace KursovaWork.Domain.Attributes;

/// <summary>
/// Validation attribute to check if the year is in the future or current.
/// </summary>
public class FutureYearAttribute : ValidationAttribute
{
    /// <summary>
    /// Method to validate if the year is in the future or current.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="validationContext">The context in which validation is performed.</param>
    /// <returns>Success or failure of the validation.</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        return value is null || !int.TryParse(value.ToString(), out var year) || year < DateTime.Now.Year % 2000
            ? new ValidationResult(ErrorMessage)
            : ValidationResult.Success;
    }
}