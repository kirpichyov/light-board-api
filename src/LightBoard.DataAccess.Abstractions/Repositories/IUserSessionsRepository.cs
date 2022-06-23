using LightBoard.Domain.Entities.Auth;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IUserSessionsRepository : IRedisRepositoryBase<UserSession, string>
{
}