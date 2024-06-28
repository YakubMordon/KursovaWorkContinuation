using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KursovaWorkBLL.Services.AdditionalServices
{
    /// <summary>
    /// Class for retrieving user ID.
    /// </summary>
    public class IDRetriever
    {
        private static readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole())
            .CreateLogger(typeof(IDRetriever));

        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the IDRetriever class.
        /// </summary>
        /// <param name="httpContextAccessor">The IHttpContextAccessor object.</param>
        public IDRetriever(IHttpContextAccessor httpContextAccessor)
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

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogInformation("User is logged in");
                return userId;
            }

            _logger.LogInformation("User is not logged in");
            return 0;
        }
    }
}
