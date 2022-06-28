using LightBoard.DataAccess.Abstractions.Arguments;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Boards;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class BoardsRepository : RelationalRepositoryBase<Board, Guid>, IBoardsRepository
{
    public BoardsRepository(PostgreSqlContext context) 
        : base(context)
    {
    }

    public async Task<Board> GetForUser(Guid boardId, Guid userId)
    {
        return await Context.Boards
                   .Include(board => board.Columns)
                   .ThenInclude(columns => columns.Cards)
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

    public async Task<IReadOnlyCollection<Card>> SearchForUser(Guid boardId, SearchCardsArgs searchCardsArgs)
    {
        var query = Context.Cards
            .AsNoTracking()
            .Include(card => card.CardAssignees)
            .ThenInclude(cardAssignee => cardAssignee.User)
            .Include(card => card.Attachments)
            .Where(card => card.Column.BoardId == boardId);

        searchCardsArgs.SearchInTitle ??= true;
        searchCardsArgs.SearchInDescription ??= true;

        if (searchCardsArgs.SearchInTitle == true)
        {
            query = query.Where(card => EF.Functions.ILike(card.Title, $"%{searchCardsArgs.Text}%"));
        }

        if (searchCardsArgs.SearchInDescription == true)
        {
            query = query.Where(card => EF.Functions.ILike(card.Description, $"%{searchCardsArgs.Text}%"));
        }

        return await query.ToArrayAsync();
    }
}