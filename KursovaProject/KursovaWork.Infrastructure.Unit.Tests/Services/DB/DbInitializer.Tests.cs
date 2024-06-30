using FluentAssertions;
using KursovaWork.Infrastructure.Services.DB;
using KursovaWork.Infrastructure.Unit.Tests.Fakers;
using Microsoft.EntityFrameworkCore;

namespace KursovaWork.Infrastructure.Unit.Tests.Services.DB;

public class DbInitializerTests
{
    [Fact]
    public void Initialize_ShouldNotAddData_WhenDatabaseAlreadyHasCars()
    {
        // Arrange

        var options = new DbContextOptionsBuilder<CarSaleContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_DbInitializer_NotEmpty")
            .Options;

        using (var context = new CarSaleContext(options))
        {
            var carFaker = new CarFaker();

            var car = carFaker.Generate();

            context.Cars.Add(car);
            context.SaveChanges();
        }

        // Act
        using (var context = new CarSaleContext(options))
        {
            DbInitializer.Initialize(context);
        }

        // Assert
        using (var context = new CarSaleContext(options))
        {
            context.Cars.Should().HaveCount(1);
            context.Users.Should().BeEmpty();

            context.Cars.RemoveRange(context.Cars);
            context.Users.RemoveRange(context.Users);
            context.SaveChanges();
        }
    }

    [Fact]
    public void Initialize_ShouldAddData_WhenDatabaseIsEmpty()
    {
        // Arrange

        var options = new DbContextOptionsBuilder<CarSaleContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_DbInitializer_Empty")
            .Options;

        // Act
        using (var context = new CarSaleContext(options))
        {
            DbInitializer.Initialize(context);
        }

        // Assert
        using (var context = new CarSaleContext(options))
        {
            context.Cars.Should().NotBeEmpty();
            context.Users.Should().NotBeEmpty();
            context.Cars.Should().HaveCount(7);
            context.Users.Should().HaveCount(10);
        }
    }
}
