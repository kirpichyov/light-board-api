﻿using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Columns;
using LightBoard.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class ColumnsRepository : RelationalRepositoryBase<Column, Guid>, IColumnsRepository
{
    public ColumnsRepository(PostgreSqlContext context)
        : base(context)
    {
    }

    public async Task<Column> GetColumnForUserById(Guid columnId, Guid userId)
    {
        return await Context.Columns
                   .Include(column => column.Board)
                   .ThenInclude(board => board.Columns)
                   .Include(column => column.Cards)
                   .ThenInclude(card => card.Attachments)
                   .SingleOrDefaultAsync(column => column.Id == columnId && column.Board.BoardMembers
                   .Any(member => member.UserId == userId))
               ?? throw new NotFoundException("Column not found");
    }
}