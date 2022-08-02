using LightBoard.Domain.Entities.Cards;
using LightBoard.Shared.Models.Enums;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface ICardsRepository : IRelationalRepositoryBase<Card, Guid>
{
    Task<Card> GetCardForUserById(Guid cardId, Guid userId);
    Task<IReadOnlyCollection<Card>> GetFilteredCards(Guid userId, Guid boardId, Guid[]? assignees,
        SortingDirection? direction, CardsSortProperty? cardsSortProperty);
    Task<bool> IsUserHasAccess(Guid cardId, Guid userId);
}