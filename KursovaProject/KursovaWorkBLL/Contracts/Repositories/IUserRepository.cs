using KursovaWork.Domain.Entities;

namespace KursovaWork.Application.Contracts.Repositories;

/// <summary>
/// Interface for handling user-related queries.
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    /// Method to retrieve a user by their identifier.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>The user.</returns>
    User GetById(int id);

    /// <summary>
    /// Method to retrieve a user by their email.
    /// </summary>
    /// <param name="email">Email address.</param>
    /// <returns>The user.</returns>
    User GetByEmail(string email);
}