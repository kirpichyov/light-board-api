using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Shared.Exceptions;
using LightBoard.Shared.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class CardRepository : RelationalRepositoryBase<Card, Guid>, ICardsRepository
{
    public CardRepository(PostgreSqlContext context) 
        : base(context)
    {
    }

    public async Task<Card> GetCardForUserById(Guid cardId, Guid userId)
    {
        return await Context.Cards
                   .Include(card => card.Attachments)
                   .Include(card => card.Column)
                   .ThenInclude(column => column.Cards)
                   .Include(card => card.CardAssignees)
                   .ThenInclude(cardAssignee => cardAssignee.User)
                   .SingleOrDefaultAsync(card => card.Id == cardId && 
                                                 card.Column.Board.BoardMembers.Any(member => member.UserId == userId))
               ?? throw new NotFoundException("Card not found");
    }

    public async Task<IReadOnlyCollection<Card>> GetFilteredCards(Guid userId, Guid boardId, Guid[]? assignees, 
        SortingDirection? direction)
    {
        var query = Context.Cards
            .Include(card => card.Column)
            .Include(card => card.CardAssignees)
            .ThenInclude(cardAssignee => cardAssignee.User)
            .Where(card =>
                card.Column.BoardId == boardId
                && card.Column.Board.BoardMembers.Any(member => member.UserId == userId));

        if (assignees is not null)
        {
            query = query.Where(card => card.CardAssignees.Any(cardAssignee => assignees.Contains(cardAssignee.Id)));
        }

        query = direction switch
        {
            SortingDirection.Desc => query.OrderByDescending(card => card.Title),
            SortingDirection.Asc => query.OrderBy(card => card.Title),
            _ => query
        };

        return await query.ToArrayAsync();
    }

    public async Task<bool> IsUserHasAccess(Guid cardId, Guid userId)
    {
        return await Context.Cards.AnyAsync(card => card.Id == cardId && card.Column.Board.BoardMembers.Any(member => member.UserId == userId));
    }
}