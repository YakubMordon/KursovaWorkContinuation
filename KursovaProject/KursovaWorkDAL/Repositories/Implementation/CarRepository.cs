using KursovaWorkDAL.Entity;
using KursovaWorkDAL.Entity.Entities.Car;
using KursovaWorkDAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace KursovaWorkDAL.Repositories.Implementation
{
    /// <summary>
    /// Implementation of the interface for handling car-related queries.
    /// </summary>
    public class CarRepository : BaseRepository<CarInfo>, ICarRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CarRepository"/> class.
        /// </summary>
        /// <param name="context">Context for database operations.</param>
        public CarRepository(CarSaleContext context) : base(context) { }

        public new IEnumerable<CarInfo> GetAll()
        {
            return _context.Cars
                .Include(c => c.Detail)
                .Include(c => c.Images).ToList();
        }
        public CarInfo GetByCarInfo(string make, string model, int year)
        {
            return _context.Cars
                .Include(c => c.Detail)
                .Include(c => c.Images)
                .FirstOrDefault(c => c.Make == make && c.Model == model && c.Year == year);
        }
        public CarInfo GetById(int id)
        {
            return _context.Cars
                .Include(c => c.Detail)
                .Include(c => c.Images)
                .FirstOrDefault(c => c.Id == id);
        }
    }
}
