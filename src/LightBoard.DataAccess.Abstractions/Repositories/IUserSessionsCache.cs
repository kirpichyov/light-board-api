using LightBoard.Domain.Entities.Auth;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IUserSessionsCache : IRedisRepositoryBase<UserSession, string>
{
    Task<UserSession?> FetchAsync(string sessionKey);
}