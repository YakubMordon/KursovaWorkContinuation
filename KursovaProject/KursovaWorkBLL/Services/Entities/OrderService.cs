using KursovaWorkBLL.Contracts;
using KursovaWorkBLL.Services.Helpers.Transient;
using KursovaWorkDAL.Entity.Entities;
using KursovaWorkDAL.Entity.Entities.Car;
using KursovaWorkDAL.Repositories.Contracts;
using Serilog;

namespace KursovaWorkBLL.Services.Entities;

/// <summary>
/// Implementation of the IOrderService interface for business logic related to orders
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IdRetriever _idRetriever;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderService"/> class.
    /// </summary>
    /// <param name="orderRepository">Repository for orders.</param>
    /// <param name="idRetriever">Service to retrieve user identifier.</param>
    public OrderService(IOrderRepository orderRepository, IdRetriever idRetriever)
    {
        _orderRepository = orderRepository;
        _idRetriever = idRetriever;
    }

    public void AddOrder(Order order)
    {
        _orderRepository.Add(order);
        Log.Information("Order successfully added");
    }

    public int AddOrderLoggedIn(CarInfo curCar, ConfiguratorOptions? configurator)
    {
        Log.Information("Retrieving user identifier");
        var loggedInUserId = _idRetriever.GetLoggedInUserId();

        var order = new Order()
        {
            CarId = curCar.Id,
            UserId = loggedInUserId,
            Price = curCar.Price,
            OrderDate = DateTime.Now
        };

        Log.Information("Creating car order");

        if (configurator != null)
        {
            order.ConfiguratorOptions = configurator;
            Log.Information("Car selected in configurator");
        }

        _orderRepository.Add(order);
        Log.Information("Order successfully added");

        return order.Id;
    }

    public void DeleteOrder(Order order)
    {
        _orderRepository.Delete(order);
        Log.Information("Order successfully deleted");
    }

    public IEnumerable<Order> FindAll(int id)
    {
        Log.Information("Retrieving orders associated with user");
        return _orderRepository.FindAll(id);
    }

    public IEnumerable<Order> FindAllLoggedIn()
    {
        Log.Information("Retrieving identifier of logged-in user");
        var loggedInId = _idRetriever.GetLoggedInUserId();

        Log.Information("Retrieving orders associated with logged-in user");
        return _orderRepository.FindAll(loggedInId);
    }

    public IEnumerable<Order> GetAll()
    {
        Log.Information("Retrieving all orders");
        return _orderRepository.GetAll();
    }

    public Order GetOrderById(int id)
    {
        Log.Information("Retrieving order by its identifier");
        return _orderRepository.GetById(id);
    }

    public void UpdateOrder(Order order)
    {
        _orderRepository.Update(order);
        Log.Information("Order successfully updated");
    }
}