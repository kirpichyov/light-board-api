using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class CardRepository : RelationalRepositoryBase<Card, Guid>, ICardsRepository
{
    public CardRepository(PostgreSqlContext context) 
        : base(context)
    {
    }

    public async Task<Card> GetForUser(Guid id, Guid userId)
    {
        return await Context.Cards
                   .Include(card => card.Attachments)
                   .Include(card => card.Column)
                   .ThenInclude(column => column.Cards)
                   .Include(card => card.CardAssignees)
                   .ThenInclude(cardAssignee => cardAssignee.User)
                   .SingleOrDefaultAsync(card => card.Id == id && 
                                                 card.Column.Board.BoardMembers.Any(member =>member.UserId == userId))
               ?? throw new NotFoundException("Card not found");
    }

    public async Task<bool> IsUserHasAccess(Guid id, Guid userId)
    {
        return await Context.Cards.AnyAsync(card => card.Id == id && card.Column.Board.BoardMembers.Any(member => member.UserId == userId));
    }
}