using KursovaWorkDAL.Entity.Entities;
using KursovaWorkDAL.Repositories.Contracts;
using KursovaWorkBLL.Contracts;
using KursovaWorkBLL.Services.Helpers.Transient;
using Serilog;

namespace KursovaWorkBLL.Services.Entities;

/// <summary>
/// Implementation of the ICardService interface for business logic related to cards
/// </summary>
public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;
    private readonly IdRetriever _idRetriever;

    /// <summary>
    /// Initializes a new instance of the <see cref="CardService"/> class.
    /// </summary>
    /// <param name="cardRepository">Repository for credit cards.</param>
    /// <param name="idRetriever">Service to retrieve user identifier.</param>
    public CardService(ICardRepository cardRepository, IdRetriever idRetriever)
    {
        _cardRepository = cardRepository;
        _idRetriever = idRetriever;
    }

    public Card GetById(int id)
    {
        Log.Information("Fetching payment method of a specific user");
        return _cardRepository.GetById(id);
    }

    public Card GetByLoggedInUser()
    {
        Log.Information("Fetching user identifier");
        var loggedInUserId = _idRetriever.GetLoggedInUserId();

        Log.Information("Fetching payment method of the logged-in user");
        return _cardRepository.GetById(loggedInUserId);
    }

    public void AddCard(Card card)
    {
        Log.Information("Fetching user identifier");
        var loggedInUserId = _idRetriever.GetLoggedInUserId();

        card.UserId = loggedInUserId;

        _cardRepository.Add(card);

        Log.Information("Payment method successfully added");
    }

    public void UpdateCard(Card card)
    {
        _cardRepository.Update(card);
        Log.Information("Payment method successfully updated");
    }

    public void DeleteCard()
    {
        Log.Information("Fetching user identifier");
        var loggedInUserId = _idRetriever.GetLoggedInUserId();

        Log.Information("Searching for payment method in the database");
        var card = _cardRepository.GetById(loggedInUserId);
        if (card is not null)
        {
            _cardRepository.Delete(card);
            Log.Information("Payment method successfully deleted");
        }
    }

    public IEnumerable<Card> GetAllCards()
    {
        Log.Information("Fetching all payment methods");
        return _cardRepository.GetAll();
    }

    public bool CardExists()
    {
        Log.Information("Fetching user identifier");
        var loggedInUserId = _idRetriever.GetLoggedInUserId();

        Log.Information("Checking if the user has a connected payment method");
        return _cardRepository.IsExisting(loggedInUserId);
    }
}