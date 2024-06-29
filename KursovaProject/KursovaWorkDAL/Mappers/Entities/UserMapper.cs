using KursovaWork.Domain.Entities;
using KursovaWork.Domain.Models;
using Riok.Mapperly.Abstractions;

namespace KursovaWork.Infrastructure.Mappers.Entities;

/// <summary>
/// Mapper class for <see cref="User"> Entity and <see cref="SignUpViewModel"/>.
/// </summary>
[Mapper]
public partial class UserMapper
{
    /// <summary>
    /// Method for mapping <see cref="User"> Entity to <see cref="SignUpViewModel"/>.
    /// </summary>
    /// <param name="entity">Entity for mapping.</param>
    /// <returns>Mapped model.</returns>
    [MapperIgnoreSource(nameof(User.Id))]
    [MapperIgnoreSource(nameof(User.CreatedAt))]
    [MapperIgnoreSource(nameof(User.Orders))]
    [MapperIgnoreSource(nameof(User.CreditCard))]
    public partial SignUpViewModel EntityToModel(User entity);

    /// <summary>
    /// Method for mapping <see cref="SignUpViewModel"/> to <see cref="User"> Entity.
    /// </summary>
    /// <param name="model">Model.</param>
    /// <returns>Mapped Entity.</returns>
    [MapperIgnoreTarget(nameof(User.Id))]
    [MapperIgnoreTarget(nameof(User.CreatedAt))]
    [MapperIgnoreTarget(nameof(User.Orders))]
    [MapperIgnoreTarget(nameof(User.CreditCard))]
    public partial User ModelToEntity(SignUpViewModel model);
}
