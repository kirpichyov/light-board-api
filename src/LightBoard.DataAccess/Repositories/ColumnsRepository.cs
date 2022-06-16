using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Columns;
using LightBoard.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class ColumnsRepository : RepositoryBase<Column, Guid>, IColumnsRepository
{
    public ColumnsRepository(PostgreSqlContext context)
        : base(context)
    {
    }

    public async Task<Column> GetForUser(Guid id, Guid userId)
    {
        return await Context.Columns.Include(column => column.Board)
                   .ThenInclude(board => board.Columns)
                   .Include(column => column.Cards)
                   .SingleOrDefaultAsync(column => column.Id == id && column.Board.BoardMembers
                   .Any((member => member.UserId == userId)))
               ?? throw new NotFoundException("Column not found");
    }
}