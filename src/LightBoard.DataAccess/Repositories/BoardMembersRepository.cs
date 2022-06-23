using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Boards;
using LightBoard.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class BoardMembersRepository : RelationalRepositoryBase<BoardMember, Guid>, IBoardMembersRepository
{
    public BoardMembersRepository(PostgreSqlContext context) 
        : base(context)
    {
    }

    public async Task<BoardMember> GetById(Guid id)
    {
        return await Context.BoardMembers.SingleOrDefaultAsync(boardMember => boardMember.Id == id)
               ?? throw new NotFoundException("Board member not found");
    }
}