using System.Collections.Concurrent;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.Domain.Entities.Auth;

namespace LightBoard.DataAccess.Repositories;

public class InMemoryUserSessionsRepository : IUserSessionsRepository
{
    private readonly ConcurrentDictionary<string, UserSession> _sessions;

    public InMemoryUserSessionsRepository()
    {
        _sessions = new ConcurrentDictionary<string, UserSession>();
    }

    public Task<UserSession?> GetAsync(string key)
    {
        _sessions.TryGetValue(key, out var session);
        return Task.FromResult(session);
    }

    public Task AddAsync(UserSession entity)
    {
        _sessions.TryAdd(entity.Key, entity);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _sessions.TryRemove(key, out _);
        return Task.CompletedTask;
    }
}