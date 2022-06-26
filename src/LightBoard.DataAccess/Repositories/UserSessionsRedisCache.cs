using LightBoard.DataAccess.Abstractions;
using LightBoard.DataAccess.Abstractions.Connection;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.Domain.Entities.Auth;
using Microsoft.Extensions.Logging;

namespace LightBoard.DataAccess.Repositories;

public class UserSessionsRedisCache : RedisRepositoryBase<UserSession, string>, IUserSessionsCache
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UserSessionsRedisCache(
        IRedisContext context,
        ILogger<RedisRepositoryBase<UserSession, string>> logger,
        IUnitOfWork unitOfWork)
        : base(context, logger)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserSession?> FetchAsync(string sessionKey)
    {
        var cachedSession = await GetAsync(sessionKey);

        if (cachedSession is not null)
        {
            return cachedSession;
        }

        var session = await _unitOfWork.UserSessions.GetBySessionKey(sessionKey);
        if (session is null)
        {
            return null;
        }

        var isExpired = DateTime.UtcNow >= session.ExpiresAtUtc;
        
        var lifeTime = isExpired
            ? TimeSpan.FromMinutes(15)
            : session.ExpiresAtUtc - DateTime.UtcNow;

        await AddAsync(session, lifeTime);

        return session;
    }

    protected override string GenerateRedisKey(string identifier) => "table.user_sessions:" + identifier;
}