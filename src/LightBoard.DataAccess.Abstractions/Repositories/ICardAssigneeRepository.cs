using LightBoard.Domain.Entities.Cards;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface ICardAssigneeRepository : IRepositoryBase<CardAssignee, Guid>
{
    Task<CardAssignee> GetById(Guid id);
}