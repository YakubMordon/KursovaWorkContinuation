using KursovaWorkDAL.Entity.Entities;
using KursovaWorkBLL.Services.AdditionalServices;
using Microsoft.Extensions.Logging;
using KursovaWorkDAL.Repositories.Contracts;
using KursovaWorkBLL.Contracts;

namespace KursovaWorkBLL.Services.Entities
{
    /// <summary>
    /// Implementation of the ICardService interface for business logic related to cards
    /// </summary>
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly ILogger<CardService> _logger;
        private readonly IDRetriever _idRetriever;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardService"/> class.
        /// </summary>
        /// <param name="cardRepository">Repository for credit cards.</param>
        /// <param name="logger">Logger for logging.</param>
        /// <param name="idRetriever">Service to retrieve user identifier.</param>
        public CardService(ICardRepository cardRepository, ILogger<CardService> logger, IDRetriever idRetriever)
        {
            _cardRepository = cardRepository;
            _logger = logger;
            _idRetriever = idRetriever;
        }

        public Card GetById(int id)
        {
            _logger.LogInformation("Fetching payment method of a specific user");
            return _cardRepository.GetById(id);
        }

        public Card GetByLoggedInUser()
        {
            _logger.LogInformation("Fetching user identifier");
            int loggedInUserId = _idRetriever.GetLoggedInUserId();

            _logger.LogInformation("Fetching payment method of the logged-in user");
            return _cardRepository.GetById(loggedInUserId);
        }

        public void AddCard(Card card)
        {
            _logger.LogInformation("Fetching user identifier");
            int loggedInUserId = _idRetriever.GetLoggedInUserId();

            card.UserId = loggedInUserId;

            _cardRepository.Add(card);

            _logger.LogInformation("Payment method successfully added");
        }

        public void UpdateCard(Card card)
        {
            _cardRepository.Update(card);
            _logger.LogInformation("Payment method successfully updated");
        }

        public void DeleteCard()
        {
            _logger.LogInformation("Fetching user identifier");
            int loggedInUserId = _idRetriever.GetLoggedInUserId();

            _logger.LogInformation("Searching for payment method in the database");
            var card = _cardRepository.GetById(loggedInUserId);
            if (card is not null)
            {
                _cardRepository.Delete(card);
                _logger.LogInformation("Payment method successfully deleted");
            }
        }

        public IEnumerable<Card> GetAllCards()
        {
            _logger.LogInformation("Fetching all payment methods");
            return _cardRepository.GetAll();
        }

        public bool CardExists()
        {
            _logger.LogInformation("Fetching user identifier");
            int loggedInUserId = _idRetriever.GetLoggedInUserId();

            _logger.LogInformation("Checking if the user has a connected payment method");
            return _cardRepository.IsExisting(loggedInUserId);
        }
    }
}
