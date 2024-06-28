using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace KursovaWorkBLL.Services.Helpers.Transient;

/// <summary>
/// Class for retrieving user ID.
/// </summary>
public class IdRetriever
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the IDRetriever class.
    /// </summary>
    /// <param name="httpContextAccessor">The IHttpContextAccessor object.</param>
    public IdRetriever(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Retrieves the ID of the logged-in user.
    /// </summary>
    /// <returns>The user ID, or 0 if the user is not logged in.</returns>
    public int GetLoggedInUserId()
    {
        var claimsPrincipal = _httpContextAccessor.HttpContext.User;
        var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
        {
            Log.Information("User is logged in");
            return userId;
        }

        Log.Information("User is not logged in");
        return 0;
    }
}