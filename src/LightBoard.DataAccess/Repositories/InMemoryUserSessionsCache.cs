using System.Collections.Concurrent;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.Domain.Entities.Auth;

namespace LightBoard.DataAccess.Repositories;

public class InMemoryUserSessionsCache : IUserSessionsCache
{
    private readonly ConcurrentDictionary<string, UserSession> _sessions;

    public InMemoryUserSessionsCache()
    {
        _sessions = new ConcurrentDictionary<string, UserSession>();
    }

    public Task<UserSession?> GetAsync(string key)
    {
        _sessions.TryGetValue(key, out var session);
        return Task.FromResult(session);
    }

    public Task<string> AddAsync(UserSession entity, TimeSpan? lifetime = null)
    {
        _sessions.TryAdd(entity.Id, entity);
        return Task.FromResult(entity.Id);
    }

    public Task RemoveAsync(string identifier)
    {
        _sessions.TryRemove(identifier, out _);
        return Task.CompletedTask;
    }

    public Task<UserSession?> FetchAsync(string sessionKey)
    {
        return GetAsync(sessionKey);
    }
}