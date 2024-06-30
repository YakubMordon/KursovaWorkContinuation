using KursovaWork.Domain.Entities;

namespace KursovaWork.Application.Contracts.Repositories;

/// <summary>
/// Interface for handling card-related queries.
/// </summary>
public interface ICardRepository : IBaseRepository<Card>
{
    /// <summary>
    /// Method to retrieve a payment method by user identifier.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>An instance of the Card class representing the user's payment method.</returns>
    Card GetById(int id);

    /// <summary>
    /// Method to check if a payment method is associated with a user.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>True if found, otherwise False.</returns>
    bool Exists(int id);
}