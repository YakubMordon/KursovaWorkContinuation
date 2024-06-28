using KursovaWorkDAL.Entity;
using KursovaWorkDAL.Entity.Entities;

namespace KursovaWorkDAL.Repositories.BaseRepository
{
    /// <summary>
    /// Repository for entity of type <see cref="TEntity"/>.
    /// </summary>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly CarSaleContext _context;

        /// <summary>
        /// Initializes instance of <see cref="BaseRepository{TEntity}>"/>.
        /// </summary>
        /// <param name="context">DB Context.</param>
        public BaseRepository(CarSaleContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
    }
}
