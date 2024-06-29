using KursovaWork.Domain.Entities;

namespace KursovaWork.Application.Contracts.Repositories;

/// <summary>
/// Contract for repository for entity of type <see cref="TEntity"/>.
/// </summary>
public interface IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    /// <summary>
    /// Method for adding entity of type <see cref="TEntity"/> to database.
    /// </summary>
    /// <param name="entity">Entity of type <see cref="TEntity"/>.</param>
    void Add(TEntity entity);

    /// <summary>
    /// Method for updating entity of type <see cref="TEntity"/> in database.
    /// </summary>
    /// <param name="entity">Entity of type <see cref="TEntity"/>.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Method for deleting entity of type <see cref="TEntity"/> from database.
    /// </summary>
    /// <param name="entity">Entity of type <see cref="TEntity"/>.</param>
    void Delete(TEntity entity);

    /// <summary>
    /// Method for getting all entities of type <see cref="TEntity"/> from database.
    /// </summary>
    /// <returns>List of entities of type <see cref="TEntity"/>.</returns>
    IEnumerable<TEntity> GetAll();
}