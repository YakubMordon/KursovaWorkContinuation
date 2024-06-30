using FluentAssertions;
using KursovaWork.Infrastructure.Repositories;
using KursovaWork.Infrastructure.Unit.Tests.Fakers;
using Microsoft.EntityFrameworkCore;

namespace KursovaWork.Infrastructure.Unit.Tests.Repositories;

public class OrderRepositoryTests : IDisposable, IAsyncDisposable
{
    private DbContextOptions<CarSaleContext> _dbContextOptions;
    private CarSaleContext _context;
    private OrderRepository _orderRepository;
    private OrderFaker _orderFaker;

    public OrderRepositoryTests()
    {
        _orderFaker = new OrderFaker();

        _dbContextOptions = new DbContextOptionsBuilder<CarSaleContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_Orders")
            .Options;

        _context = new CarSaleContext(_dbContextOptions);
        _orderRepository = new OrderRepository(_context);
    }

    [Fact]
    public void Add_DataIsCorrect_ShouldAddOrder()
    {
        // Arrange

        var entity = _orderFaker.Generate();

        // Act

        _orderRepository.Add(entity);

        // Assert

        _context.Orders.Should().Contain(entity);

        _orderRepository.Delete(entity);
    }

    [Fact]
    public void Delete_OrderExists_ShouldDeleteOrder()
    {
        // Arrange

        var entity = _orderFaker.Generate();

        // Act

        _orderRepository.Add(entity);

        _orderRepository.Delete(entity);

        // Assert

        _context.Orders.Should().NotContain(entity);
    }

    [Fact]
    public void Delete_OrderNotExists_ShouldNotDeleteOrder()
    {
        // Arrange

        var entity = _orderFaker.Generate();

        // Act

        Action removal = () => _orderRepository.Delete(entity);

        // Assert

        removal.Should().Throw<DbUpdateConcurrencyException>();
    }

    [Fact]
    public void Update_DataIsCorrect_ShouldUpdateOrder()
    {
        // Arrange

        var entity = _orderFaker.Generate();

        // Act

        _orderRepository.Add(entity);

        entity.Price = 10101;

        _orderRepository.Update(entity);

        // Assert

        _context.Orders.Should().Contain(entity);

        _orderRepository.Delete(entity);
    }    

    [Fact]
    public void GetAll_ShouldGetAllOrders()
    {
        // Arrange

        var list = _orderFaker.Generate(10);

        // Act

        _context.Orders.AddRange(list);

        _context.SaveChanges();

        var actual = _orderRepository.GetAll();

        // Assert

        actual.Should().BeEquivalentTo(list);

        _context.Orders.RemoveRange(list);

        _context.SaveChanges();
    }

    [Fact]
    public void GetById_OrderExists_ShouldReturnOrder()
    {
        // Arrange

        var entity = _orderFaker.Generate();

        // Act

        _orderRepository.Add(entity);

        var actual = _orderRepository.GetById(entity.Id);

        // Assert

        actual.Should().BeEquivalentTo(entity);

        _orderRepository.Delete(entity);
    }

    [Fact]
    public void GetById_OrderNotExists_ShouldReturnNull()
    {
        // Act

        var actual = _orderRepository.GetById(10);

        // Assert

        actual.Should().BeNull();
    }

    [Fact]
    public void FindByUserId_ShouldReturnOrders()
    {
        // Arrange

        var list = _orderFaker.Generate(50);

        // Act

        _context.Orders.AddRange(list);

        _context.SaveChanges();

        var id = list.FirstOrDefault().UserId;

        var filteredList = list.Where(order => order.UserId == id);

        var actual = _orderRepository.FindByUserId(id);

        // Assert

        actual.Should().BeEquivalentTo(filteredList);

        _context.Orders.RemoveRange(list);

        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}