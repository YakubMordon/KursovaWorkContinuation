using KursovaWorkDAL.Entity;
using KursovaWorkDAL.Entity.Entities;
using KursovaWorkDAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace KursovaWorkDAL.Repositories.Implementation
{
    /// <summary>
    /// Implementation of the interface for handling user-related queries.
    /// </summary>
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">Context for database operations.</param>
        public UserRepository(CarSaleContext context) : base(context) { }

        public new IEnumerable<User> GetAll()
        {
            return _context.Users
                 .Include(u => u.CreditCard);
        }

        public User GetByEmail(string email)
        {
            return _context.Users
                .FirstOrDefault(u => u.Email == email);
        }

        public User GetById(int id)
        {
            return _context.Users
                .FirstOrDefault(u => u.Id == id);
        }
    }
}
