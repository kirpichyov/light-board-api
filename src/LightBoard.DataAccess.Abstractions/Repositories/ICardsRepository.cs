using LightBoard.Domain.Entities.Cards;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface ICardsRepository : IRelationalRepositoryBase<Card, Guid>
{
    Task<Card> GetCardForUserById(Guid cardId, Guid userId);
    Task<bool> IsUserHasAccess(Guid cardId, Guid userId);
}