using KursovaWorkDAL.Entity;
using KursovaWorkDAL.Entity.Entities;

namespace KursovaWorkDAL.Repositories.CardRepository
{
    /// <summary>
    /// Card repository.
    /// </summary>
    public class CardRepository : BaseRepository.BaseRepository<Card>, ICardRepository
    {
        /// <summary>
        /// Initializes an instance of <see cref="CardRepository"/>.
        /// </summary>
        /// <param name="context">Database context.</param>
        public CardRepository(CarSaleContext context) : base(context) { }

        public Card GetById(int id)
        {
            return _context.Cards.FirstOrDefault(c => c.UserId == id);
        }
        public bool IsExisting(int id)
        {
            return _context.Cards.Any(u => u.UserId == id);
        }
    }
}
