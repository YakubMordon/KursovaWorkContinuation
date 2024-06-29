using KursovaWork.Application.Contracts.Repositories;
using KursovaWork.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KursovaWork.Infrastructure.Repositories;

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
        return Context.Users
            .Include(u => u.CreditCard);
    }

    public User GetByEmail(string email)
    {
        return Context.Users
            .FirstOrDefault(u => u.Email == email);
    }

    public User GetById(int id)
    {
        return Context.Users
            .FirstOrDefault(u => u.Id == id);
    }
}