using KursovaWork.Application.Contracts.Repositories;
using KursovaWork.Domain.Entities;

namespace KursovaWork.Infrastructure.Repositories;

/// <summary>
/// Repository for entity of type <see cref="TEntity"/>.
/// </summary>
public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly CarSaleContext Context;

    /// <summary>
    /// Initializes instance of <see cref="BaseRepository{TEntity}>"/>.
    /// </summary>
    /// <param name="context">DB Context.</param>
    public BaseRepository(CarSaleContext context)
    {
        Context = context;
    }

    public void Add(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);

        Context.SaveChanges();
    }

    public void Delete(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);

        Context.SaveChanges();
    }

    public IEnumerable<TEntity> GetAll()
    {
        return Context.Set<TEntity>();
    }

    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);

        Context.SaveChanges();
    }
}