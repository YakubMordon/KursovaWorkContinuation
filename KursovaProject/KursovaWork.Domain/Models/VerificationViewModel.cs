namespace KursovaWork.Domain.Models;

/// <summary>
/// Model for getting Verification code.
/// </summary>
public class VerificationViewModel
{
    /// <summary>
    /// Array of verification code's digits.
    /// </summary>
    public string[] VerificationDigits { get; set; } = new string[4];
}