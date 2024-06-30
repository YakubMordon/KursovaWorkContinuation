using KursovaWork.Domain.Entities;
using KursovaWork.Domain.Entities.Car;

namespace KursovaWork.Application.Contracts.Services.Entities;

/// <summary>
/// Interface for business logic related to orders
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Method to retrieve an order by its identifier
    /// </summary>
    /// <param name="id">Order identifier</param>
    /// <returns>Order</returns>
    Order GetOrderById(int id);

    /// <summary>
    /// Method to add an order to the database
    /// </summary>
    /// <param name="order">Order</param>
    void AddOrder(Order order);

    /// <summary>
    /// Method to add an order to the database for a logged-in user
    /// </summary>
    /// <param name="curCar">Car chosen by the user</param>
    /// <param name="configurator">Car configuration options if chosen through configurator</param>
    /// <returns>Order number</returns>
    int AddOrderLoggedIn(Car curCar, ConfiguratorOptions? configurator = null);

    /// <summary>
    /// Method to update an order in the database
    /// </summary>
    /// <param name="order">Order</param>
    void UpdateOrder(Order order);

    /// <summary>
    /// Method to delete an order from the database
    /// </summary>
    /// <param name="order">Order</param>
    void DeleteOrder(Order order);

    /// <summary>
    /// Method to retrieve all orders
    /// </summary>
    /// <returns>List of all orders</returns>
    IEnumerable<Order> GetAll();

    /// <summary>
    /// Method to retrieve orders associated with a user by a specific identifier
    /// </summary>
    /// <param name="id">User identifier</param>
    /// <returns>List of orders</returns>
    IEnumerable<Order> FindAll(int id);

    /// <summary>
    /// Method to retrieve orders associated with the currently logged-in user
    /// </summary>
    /// <returns>List of orders</returns>
    IEnumerable<Order> FindAllLoggedIn();
}