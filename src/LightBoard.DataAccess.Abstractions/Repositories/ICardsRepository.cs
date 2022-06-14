using LightBoard.Domain.Entities.Cards;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface ICardsRepository : IRepositoryBase<Card, Guid>
{
    Task<Card> GetForUser(Guid id, Guid userId);
}