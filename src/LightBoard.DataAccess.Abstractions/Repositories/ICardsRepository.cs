using LightBoard.Domain.Entities.Cards;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface ICardsRepository : IRelationalRepositoryBase<Card, Guid>
{
    Task<Card> GetForUser(Guid id, Guid userId);
    Task<bool> IsUserHasAccess(Guid id, Guid userId);
}