using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;

namespace KursovaWork.Infrastructure.Services.Helpers.Static;

/// <summary>
/// Helper class for working with ViewBag.
/// </summary>
public static class ViewBagHelper
{
    /// <summary>
    /// Sets the IsLoggedIn value in ViewBag based on user authentication information.
    /// </summary>
    /// <param name="viewContext">The ViewContext.</param>
    public static void SetIsLoggedInInViewBag(this ViewContext viewContext)
    {
        var isLoggedIn = viewContext.HttpContext.User.Identity.IsAuthenticated;
        viewContext.ViewBag.IsLoggedIn = isLoggedIn;
        Log.Information("Successfully checked if the user is logged in");
    }
}