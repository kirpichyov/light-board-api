using LightBoard.Domain.Entities.Boards;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IBoardMembersRepository : IRelationalRepositoryBase<BoardMember, Guid>
{
    Task<BoardMember> GetById(Guid id);
}