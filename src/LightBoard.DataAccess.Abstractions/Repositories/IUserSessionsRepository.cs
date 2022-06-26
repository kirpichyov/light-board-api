using LightBoard.Domain.Entities.Auth;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IUserSessionsRepository : IRelationalRepositoryBase<UserSession, string>
{
    Task<UserSession?> GetBySessionKey(string sessionKey);
    Task<IReadOnlyCollection<UserSession>> GetAllByUserId(Guid userId);
}