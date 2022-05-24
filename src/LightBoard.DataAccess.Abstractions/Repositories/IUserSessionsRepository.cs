using LightBoard.Domain.Entities.Auth;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IUserSessionsRepository
{
    Task<UserSession?> GetAsync(string sessionKey);
    Task AddAsync(UserSession session);
    Task RemoveAsync(string sessionKey);
}