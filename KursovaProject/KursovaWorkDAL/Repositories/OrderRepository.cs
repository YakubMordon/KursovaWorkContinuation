using KursovaWork.Application.Contracts.Repositories;
using KursovaWork.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KursovaWork.Infrastructure.Repositories;

/// <summary>
/// Implementation of the interface for handling order-related queries.
/// </summary>
public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderRepository"/> class.
    /// </summary>
    /// <param name="context">Context for database operations.</param>
    public OrderRepository(CarSaleContext context) : base(context) { }

    public Order GetById(int id)
    {
        return Context.Orders
            .FirstOrDefault(o => o.Id == id);
    }

    public IEnumerable<Order> FindByUserId(int id)
    {
        return Context.Orders
            .Include(o => o.Car)
                .ThenInclude(c => c.Detail)
            .Include(o => o.ConfiguratorOptions)
            .Where(o => o.UserId == id);
    }
    public new IEnumerable<Order> GetAll()
    {
        return Context.Orders
            .Include(o => o.Car)
                .ThenInclude(c => c.Detail)
            .Include(o => o.ConfiguratorOptions);
    }
}