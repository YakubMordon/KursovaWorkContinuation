using KursovaWork.Domain.Entities;

namespace KursovaWork.Application.Contracts.Repositories;

/// <summary>
/// Interface for handling orders-related queries.
/// </summary>
public interface IOrderRepository : IBaseRepository<Order>
{
    /// <summary>
    /// Method to retrieve an order by its identifier.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    /// <returns>The order.</returns>
    Order GetById(int id);

    /// <summary>
    /// Method to retrieve orders associated with a specific user identifier.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>List of orders.</returns>
    IEnumerable<Order> FindByUserId(int id);
}