using FluentAssertions;
using KursovaWork.Infrastructure.Services.Helpers.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace KursovaWork.Infrastructure.Unit.Tests.Services.Helpers.Static;

public class ViewBagHelperTests
{
    [Fact]
    public void SetIsLoggedInInViewBag_UserIsLoggedIn_ShouldSetStateOfUser()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
           new Claim(ClaimTypes.Name, "TestUser")
        }, "mock"));

        var httpContext = new DefaultHttpContext
        {
            User = user
        };

        var viewContext = new ViewContext
        {
            HttpContext = httpContext
        };

        // Act
        viewContext.SetIsLoggedInInViewBag();

        // Assert
        var actual = (bool)viewContext.ViewBag.IsLoggedIn;
        actual.Should().BeTrue();
    }

    [Fact]
    public void SetIsLoggedInInViewBag_UserIsNotLoggedIn_ShouldSetStateOfUser()
    {
        var httpContext = new DefaultHttpContext();

        var viewContext = new ViewContext
        {
            HttpContext = httpContext
        };

        // Act
        viewContext.SetIsLoggedInInViewBag();

        // Assert
        var actual = (bool)viewContext.ViewBag.IsLoggedIn;
        actual.Should().BeFalse();
    }
}
