using KursovaWork.Application.Contracts.Repositories;
using KursovaWork.Domain.Entities;

namespace KursovaWork.Infrastructure.Repositories;

/// <summary>
/// Card repository.
/// </summary>
public class CardRepository : BaseRepository<Card>, ICardRepository
{
    /// <summary>
    /// Initializes an instance of <see cref="CardRepository"/>.
    /// </summary>
    /// <param name="context">Database context.</param>
    public CardRepository(CarSaleContext context) : base(context) { }

    public Card GetById(int id)
    {
        return Context.Cards.FirstOrDefault(c => c.UserId == id);
    }

    public bool Exists(int id)
    {
        return Context.Cards.Any(u => u.UserId == id);
    }
}