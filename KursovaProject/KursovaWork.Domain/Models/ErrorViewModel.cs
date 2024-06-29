namespace KursovaWork.Domain.Models;

/// <summary>
/// Model for showing error.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Id of request where error was showed.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Method for checking if there's request id.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}