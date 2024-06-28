using KursovaWorkDAL.Entity.Entities.Car;

namespace KursovaWorkDAL.Repositories.Contracts
{
    /// <summary>
    /// Interface for handling car-related queries.
    /// </summary>
    public interface ICarRepository : IBaseRepository<CarInfo>
    {
        /// <summary>
        /// Method to retrieve car information by its identifier.
        /// </summary>
        /// <param name="id">Car identifier.</param>
        /// <returns>Car information.</returns>
        CarInfo GetById(int id);

        /// <summary>
        /// Method to retrieve car information by its make, model, and year of production.
        /// </summary>
        /// <param name="make">Car make.</param>
        /// <param name="model">Car model.</param>
        /// <param name="year">Year of car production.</param>
        /// <returns>Car information.</returns>
        CarInfo GetByCarInfo(string make, string model, int year);
    }
}
