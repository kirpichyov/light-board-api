using LightBoard.DataAccess.Abstractions.Repositories;

namespace LightBoard.DataAccess.Abstractions;

public interface IUnitOfWork
{
    IUsersRepository Users { get; }
    IBoardsRepository Boards { get; }
    IBoardMembersRepository BoardMembers { get; }
    Task SaveChangesAsync();
}