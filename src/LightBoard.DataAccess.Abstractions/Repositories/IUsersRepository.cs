using LightBoard.Domain.Entities.Auth;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IUsersRepository : IRelationalRepositoryBase<User, Guid>
{
    Task<bool> IsExists(string email);
    Task<User?> Get(string email);
    Task<User> GetById(Guid id);
}