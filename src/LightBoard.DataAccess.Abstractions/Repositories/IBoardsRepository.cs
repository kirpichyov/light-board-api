using LightBoard.Domain.Entities.Boards;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IBoardsRepository : IRepositoryBase<Board, Guid>
{
    Task<Board> GetForUser(Guid boardId, Guid userId);
    Task<IReadOnlyCollection<Board>> GetAllByUserId(Guid userId);
    Task<bool> HasAccessToBoard(Guid boardId, Guid userId);
}