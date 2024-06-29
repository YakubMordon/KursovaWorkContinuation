namespace KursovaWork.Application.Contracts.Services.Helpers.Transient;

/// <summary>
/// Contract for Id retrieving service.
/// </summary>
public interface IIdRetriever
{
    /// <summary>
    /// Retrieves the ID of the logged-in user.
    /// </summary>
    /// <returns>The user ID, or 0 if the user is not logged in.</returns>
    public int GetLoggedInUserId();
}
