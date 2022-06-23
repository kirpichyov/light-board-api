using LightBoard.DataAccess.Abstractions.Connection;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.Domain.Entities.Auth;
using Microsoft.Extensions.Logging;

namespace LightBoard.DataAccess.Repositories;

public class UserSessionsRepository : RedisRepositoryBase<UserSession, string>, IUserSessionsRepository
{
    public UserSessionsRepository(IRedisContext context, ILogger<RedisRepositoryBase<UserSession, string>> logger)
        : base(context, logger)
    {
    }

    protected override string GenerateRedisKey(string key) => "table.user_sessions:" + key;
}