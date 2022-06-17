using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class CardAssigneeRepository : RepositoryBase<CardAssignee, Guid>,  ICardAssigneeRepository
{
    public CardAssigneeRepository(PostgreSqlContext context)
        : base(context)
    {
    }

    public async Task<CardAssignee> GetById(Guid id)
    {
        return await Context.CardAssignees.AsNoTracking()
                   .Include(cardAssignee => cardAssignee.Card)
                   .SingleOrDefaultAsync(cardAssignee => cardAssignee.Id == id)
               ?? throw new NotFoundException("Card assignee not found");
    }
}