using Microsoft.AspNetCore.Http;
using Serilog;
using System.Security.Claims;
using KursovaWork.Application.Contracts.Services.Helpers.Transient;

namespace KursovaWork.Infrastructure.Services.Helpers.Transient;

/// <summary>
/// Class for retrieving user ID.
/// </summary>
public class IdRetriever : IIdRetriever
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

    public int GetLoggedInUserId()
    {
        var claimsPrincipal = _httpContextAccessor.HttpContext.User;
        var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is not null && int.TryParse(userIdClaim.Value, out var userId))
        {
            Log.Information("User is logged in");
            return userId;
        }

        Log.Information("User is not logged in");
        return 0;
    }
}