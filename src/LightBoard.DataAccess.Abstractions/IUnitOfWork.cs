using LightBoard.DataAccess.Abstractions.Repositories;

namespace LightBoard.DataAccess.Abstractions;

public interface IUnitOfWork
{
    IUsersRepository Users { get; }
    Task SaveChangesAsync();
}