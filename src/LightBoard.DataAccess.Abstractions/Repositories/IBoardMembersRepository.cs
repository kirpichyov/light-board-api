using LightBoard.Domain.Entities.Boards;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IBoardMembersRepository : IRepositoryBase<BoardMember, Guid>
{
}