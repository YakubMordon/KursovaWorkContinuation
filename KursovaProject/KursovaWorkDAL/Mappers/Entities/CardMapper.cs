using Riok.Mapperly.Abstractions;
using KursovaWork.Domain.Entities;
using KursovaWork.Domain.Models;

namespace KursovaWork.Infrastructure.Mappers.Entities;

/// <summary>
/// Mapper class for <see cref="Card"> Entity and <see cref="CreditCardViewModel"/>.
/// </summary>
[Mapper]
public partial class CardMapper
{
    /// <summary>
    /// Method for mapping <see cref="Card"> Entity to <see cref="CreditCardViewModel"/>.
    /// </summary>
    /// <param name="entity">Entity for mapping.</param>
    /// <returns>Mapped model.</returns>
    [MapperIgnoreSource(nameof(Card.User))]
    public partial CreditCardViewModel EntityToModel(Card entity);

    /// <summary>
    /// Method for mapping <see cref="CreditCardViewModel"/> to <see cref="Card"> Entity.
    /// </summary>
    /// <param name="model">Model.</param>
    /// <returns>Mapped Entity.</returns>
    [MapperIgnoreTarget(nameof(Card.User))]
    public partial Card ModelToEntity(CreditCardViewModel model);
}
