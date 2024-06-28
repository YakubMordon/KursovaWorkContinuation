using KursovaWorkDAL.Entity;
using KursovaWorkDAL.Entity.Entities;
using KursovaWorkDAL.Repositories.Contracts;

namespace KursovaWorkDAL.Repositories.Implementation
{
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
        public bool IsExisting(int id)
        {
            return Context.Cards.Any(u => u.UserId == id);
        }
    }
}
