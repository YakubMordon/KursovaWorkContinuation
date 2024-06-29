using KursovaWork.Domain.Entities;

namespace KursovaWork.Application.Contracts.Services.Entities;

/// <summary>
/// Interface for business logic related to cards
/// </summary>
public interface ICardService
{
    /// <summary>
    /// Method to retrieve payment method data by user identifier
    /// </summary>
    /// <param name="id">User identifier</param>
    /// <returns>An object of type Card. Data about the user's payment method</returns>
    Card GetById(int id);

    /// <summary>
    /// Method to retrieve payment method data of the logged-in user
    /// </summary>
    /// <returns>An object of type Card. Data about the logged-in user's payment method</returns>
    Card GetByLoggedInUser();

    /// <summary>
    /// Method to add a new payment method for the logged-in user
    /// </summary>
    /// <param name="card">Payment method</param>
    void AddCard(Card card);

    /// <summary>
    /// Method to update a payment method
    /// </summary>
    /// <param name="card">Payment method</param>
    void UpdateCard(Card card);

    /// <summary>
    /// Method to delete the payment method of the logged-in user
    /// </summary>
    void DeleteCard();

    /// <summary>
    /// Method to check if a connected payment method exists for the logged-in user
    /// </summary>
    /// <returns>True if exists, False if not</returns>
    bool CardExists();

    /// <summary>
    /// Method to retrieve all possible payment methods
    /// </summary>
    /// <returns>List of all possible payment methods</returns>
    IEnumerable<Card> GetAllCards();
}