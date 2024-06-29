using KursovaWork.Domain.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Serilog;
using KursovaWork.Application.Contracts.Services.Entities;

namespace KursovaWork.UI.Controllers;

/// <summary>
/// Controller responsible for main actions on the home page.
/// </summary>
public class HomeController : Controller
{
    private readonly IOrderService _orderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeController"/> class.
    /// </summary>
    /// <param name="orderService">The service interface for handling orders.</param>
    public HomeController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Redirects to the home page.
    /// </summary>
    /// <returns>The home page.</returns>
    public IActionResult Index()
    {
        Log.Information("Redirecting to the home page");
        return View("~/Views/Home/Index.cshtml");
    }

    /// <summary>
    /// Redirects to the login page.
    /// </summary>
    /// <returns>The login page.</returns>
    public IActionResult LogIn()
    {
        Log.Information("Redirecting to the login page");
        return View("~/Views/LogIn/LogIn.cshtml");
    }

    /// <summary>
    /// Executes logout from the account.
    /// </summary>
    /// <returns>Redirects to the home page.</returns>
    public IActionResult LogOut()
    {
        Log.Information("Executing logout from the account");
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

        Log.Information("Redirecting to the home page");
        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Redirects to the model list page.
    /// </summary>
    /// <returns>Redirects to the car model list page.</returns>
    public IActionResult ModelList()
    {
        Log.Information("Clearing unnecessary configurations");
        ConfiguratorController.Options = null;

        Log.Information("Redirecting to the model list page");
        return RedirectToAction("ModelList", "ModelList");
    }

    /// <summary>
    /// Redirects to the order list page.
    /// </summary>
    /// <returns>The order list page.</returns>
    public IActionResult OrderList()
    {
        Log.Information("Entering method to redirect to the order list page");

        var orders = _orderService
                        .FindAllLoggedIn()
                        .ToList();

        Log.Information("Redirecting to the order list page");

        return View("~/Views/OrderList/OrderList.cshtml", orders);
    }

    /// <summary>
    /// Handles errors during request execution.
    /// </summary>
    /// <returns>Page displaying the error.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        Log.Error("An error occurred while loading the site");
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}