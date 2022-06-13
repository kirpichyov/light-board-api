using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Boards;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class BoardMembersRepository : RepositoryBase<BoardMember, Guid>, IBoardMembersRepository
{
    public BoardMembersRepository(PostgreSqlContext context) 
        : base(context)
    {
    }
}