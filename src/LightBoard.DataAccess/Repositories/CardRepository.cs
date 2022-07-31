using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Shared.Exceptions;
using LightBoard.Shared.Extensions;
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
        SortingDirection? direction, CardsSortProperty? sortBy)
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

        if (sortBy is not null)
        {
            query = sortBy switch
            {
                CardsSortProperty.Name => query.SortBy(cards => cards.Title, direction),
                CardsSortProperty.Priority => query.SortBy(cards => cards.Priority, direction),
                _ => query
            };
        }
        else
        {
            query = query.SortBy(cards => cards.Title, direction ?? SortingDirection.Asc);
        }

        return await query.ToArrayAsync();
    }

    public async Task<bool> IsUserHasAccess(Guid cardId, Guid userId)
    {
        return await Context.Cards.AnyAsync(card => card.Id == cardId && card.Column.Board.BoardMembers.Any(member => member.UserId == userId));
    }
}