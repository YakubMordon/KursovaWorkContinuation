using FluentAssertions;
using KursovaWork.Infrastructure.Services.Helpers.Transient;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace KursovaWork.Infrastructure.Unit.Tests.Services.Helpers.Transient;

public class IdRetrieverTests
{
    [Fact]
    public void GetLoggedInUserId_UserIsLoggedIn_ShouldReturnUserId()
    {
        // Arrange
        var expectedUserId = 123;

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, expectedUserId.ToString())
        }));

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var idRetriever = new IdRetriever(httpContextAccessorMock.Object);

        // Act
        var actualUserId = idRetriever.GetLoggedInUserId();

        // Assert
        actualUserId.Should().Be(expectedUserId);
    }

    [Fact]
    public void GetLoggedInUserId_UserIsNotLoggedIn_ShouldReturnZero()
    {
        // Arrange
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var idRetriever = new IdRetriever(httpContextAccessorMock.Object);

        // Act
        var actualUserId = idRetriever.GetLoggedInUserId();

        // Assert
        actualUserId.Should().Be(0);
    }

    [Fact]
    public void GetLoggedInUserId_UserIdClaimIsNotValidInteger_ShouldReturnZero()
    {
        // Arrange

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "invalid_id")
        }));

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var idRetriever = new IdRetriever(httpContextAccessorMock.Object);

        // Act
        var actualUserId = idRetriever.GetLoggedInUserId();

        // Assert
        actualUserId.Should().Be(0);
    }
}
