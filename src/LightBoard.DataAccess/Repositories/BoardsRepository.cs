using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Boards;
using LightBoard.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class BoardsRepository : RepositoryBase<Board, Guid>, IBoardsRepository
{
    public BoardsRepository(PostgreSqlContext context) 
        : base(context)
    {
    }

    public async Task<Board> GetForUser(Guid boardId, Guid userId)
    {
        return await Context.Boards
                   .Include(board => board.Columns)
                   .Include(board => board.BoardMembers)
                   .ThenInclude(member => member.User)
                   .SingleOrDefaultAsync(board => board.Id == boardId && board.BoardMembers
                       .Any(member => member.UserId == userId))
               ?? throw new NotFoundException("Board not found");
    }

    public async Task<IReadOnlyCollection<Board>> GetAllByUserId(Guid userId)
    {
        return await Context.Boards.Include(board => board.Columns)
            .Where(board => board.BoardMembers
            .Any(boardMember => boardMember.UserId == userId))
            .ToArrayAsync();
    }

    public async Task<bool> HasAccessToBoard(Guid boardId, Guid userId)
    {
        return await Context.Boards.AnyAsync(board => board.Id == boardId && board.BoardMembers
            .Any(member => member.UserId == userId));
    }
}