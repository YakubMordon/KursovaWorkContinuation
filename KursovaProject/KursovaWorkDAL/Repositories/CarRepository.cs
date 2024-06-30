using KursovaWork.Application.Contracts.Repositories;
using KursovaWork.Domain.Entities.Car;
using Microsoft.EntityFrameworkCore;

namespace KursovaWork.Infrastructure.Repositories;

/// <summary>
/// Implementation of the interface for handling car-related queries.
/// </summary>
public class CarRepository : BaseRepository<Car>, ICarRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CarRepository"/> class.
    /// </summary>
    /// <param name="context">Context for database operations.</param>
    public CarRepository(CarSaleContext context) : base(context) { }

    public new IEnumerable<Car> GetAll()
    {
        return Context.Cars
            .Include(c => c.Detail)
            .Include(c => c.Images).ToList();
    }
    public Car GetByCarInfo(string make, string model, int year)
    {
        return Context.Cars
            .Include(c => c.Detail)
            .Include(c => c.Images)
            .FirstOrDefault(c => c.Make == make && c.Model == model && c.Year == year);
    }
    public Car GetById(int id)
    {
        return Context.Cars
            .Include(c => c.Detail)
            .Include(c => c.Images)
            .FirstOrDefault(c => c.Id == id);
    }
}