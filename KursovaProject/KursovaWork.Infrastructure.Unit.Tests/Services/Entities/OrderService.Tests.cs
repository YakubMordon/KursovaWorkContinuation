using FluentAssertions;
using KursovaWork.Application.Contracts.Repositories;
using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Application.Contracts.Services.Helpers.Transient;
using KursovaWork.Domain.Entities;
using KursovaWork.Infrastructure.Services.Entities;
using KursovaWork.Infrastructure.Unit.Tests.Fakers;
using Moq;

namespace KursovaWork.Infrastructure.Unit.Tests.Services.Entities;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IIdRetriever> _mockIdRetriever;
    private readonly IOrderService _orderService;
    private readonly OrderFaker _orderFaker;
    public OrderServiceTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockIdRetriever = new Mock<IIdRetriever>();
        _orderService = new OrderService(_mockOrderRepository.Object, _mockIdRetriever.Object);

        _orderFaker = new OrderFaker();
    }

    [Fact]
    public void GetById_EntityExists_ShouldReturnCorrectOrder()
    {
        // Arrange
        var expectedOrder = _orderFaker.Generate();
        _mockOrderRepository.Setup(repo => repo.GetById(expectedOrder.Id)).Returns(expectedOrder);

        // Act
        var result = _orderService.GetOrderById(expectedOrder.Id);

        // Assert
        result.Should().BeEquivalentTo(expectedOrder);
    }

    [Fact]
    public void GetById_EntityNotExists_ShouldReturnNull()
    {
        // Arrange
        int id = 1;
        _mockOrderRepository.Setup(repo => repo.GetById(1)).Returns<Order?>(null);

        // Act
        var result = _orderService.GetOrderById(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void FindAllLoggedIn_EntityExists_ShouldReturnCorrectOrders()
    {
        // Arrange
        var list = _orderFaker.Generate(10);

        var userId = list.First().UserId;

        var expected = list.Where(order => order.UserId == userId).ToList();
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(userId);
        _mockOrderRepository.Setup(repo => repo.FindByUserId(userId)).Returns(expected);

        // Act
        var result = _orderService.FindAllLoggedIn();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void FindAllLoggedIn_EntityNotExists_ShouldReturnNull()
    {
        // Arrange
        var id = 0;
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(id);
        _mockOrderRepository.Setup(repo => repo.FindByUserId(id)).Returns<Order?>(null);

        // Act
        var result = _orderService.FindAllLoggedIn();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void FindAll_EntityExists_ShouldReturnCorrectOrders()
    {
        // Arrange
        var list = _orderFaker.Generate(10);

        var userId = list.First().UserId;

        var expected = list.Where(order => order.UserId == userId).ToList();

        _mockOrderRepository.Setup(repo => repo.FindByUserId(userId)).Returns(expected);

        // Act
        var result = _orderService.FindAll(userId);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void FindAll_EntityNotExists_ShouldReturnNull()
    {
        // Arrange
        var id = 0;
        _mockOrderRepository.Setup(repo => repo.FindByUserId(id)).Returns<Order?>(null);

        // Act
        var result = _orderService.FindAll(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void AddOrder_ShouldSucceed()
    {
        // Arrange
        var orderToAdd = _orderFaker.Generate();
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(orderToAdd.UserId);

        // Act
        _orderService.AddOrder(orderToAdd);

        // Assert
        _mockOrderRepository.Verify(repo => repo.Add(orderToAdd), Times.Once);
    }

    [Fact]
    public void AddOrderLoggedIn_WithoutConfiguration_ShouldSucceed()
    {
        // Arrange
        var orderToAdd = _orderFaker.Generate();
        var carFaker = new CarFaker();

        var car = carFaker.Generate();
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(orderToAdd.UserId);

        // Act
        _orderService.AddOrderLoggedIn(car);

        // Assert
        _mockOrderRepository.Verify(repo => repo.Add(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public void AddOrderLoggedIn_WithConfiguration_ShouldSucceed()
    {
        // Arrange
        var orderToAdd = _orderFaker.Generate();
        var carFaker = new CarFaker();

        var car = carFaker.Generate();

        var configuration = new ConfiguratorOptions
        {
            Id = 1,
            OrderId = orderToAdd.Id,
            Color = "Black",
            Transmission = "Automatic",
            FuelType = "Gasoline"
        };

        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(orderToAdd.UserId);

        // Act
        _orderService.AddOrderLoggedIn(car, configuration);

        // Assert
        _mockOrderRepository.Verify(repo => repo.Add(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public void UpdateOrder_ShouldSucceed()
    {
        // Arrange
        var orderToUpdate = _orderFaker.Generate();

        // Act
        _orderService.UpdateOrder(orderToUpdate);

        // Assert
        _mockOrderRepository.Verify(repo => repo.Update(orderToUpdate), Times.Once);
    }

    [Fact]
    public void DeleteOrder_ShouldSucceed()
    {
        // Arrange
        var orderToDelete = _orderFaker.Generate();
        _mockIdRetriever.Setup(ir => ir.GetLoggedInUserId()).Returns(orderToDelete.UserId);
        _mockOrderRepository.Setup(repo => repo.GetById(orderToDelete.UserId)).Returns(orderToDelete);

        // Act
        _orderService.DeleteOrder(orderToDelete);

        // Assert
        _mockOrderRepository.Verify(repo => repo.Delete(orderToDelete), Times.Once);
    }

    [Fact]
    public void GetAllOrders_ShouldReturnAllOrders()
    {
        // Arrange
        var expectedOrders = _orderFaker.Generate(5);
        _mockOrderRepository.Setup(repo => repo.GetAll()).Returns(expectedOrders);

        // Act
        var result = _orderService.GetAll();

        // Assert
        result.Should().BeEquivalentTo(expectedOrders);
    }
}
