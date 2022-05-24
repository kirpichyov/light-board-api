using LightBoard.DataAccess.Abstractions.Connection;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.Domain.Entities.Auth;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LightBoard.DataAccess.Repositories;

public class UserSessionsRepository : IUserSessionsRepository
{
    private readonly IRedisContext _context;
    private readonly ILogger<UserSessionsRepository> _logger;

    public UserSessionsRepository(IRedisContext context, ILogger<UserSessionsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserSession?> GetAsync(string sessionKey)
    {
        var redisValue = await _context.Database.StringGetAsync(BuildKey(sessionKey));

        if (!redisValue.HasValue)
        {
            return null;
        }

        string jsonValue = redisValue.ToString();

        try
        {
            return JsonConvert.DeserializeObject<UserSession>(jsonValue);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to deserialize redis json value. Value: '{jsonValue}'", jsonValue);
            throw;
        }
    }

    public async Task AddAsync(UserSession session)
    {
        string? jsonValue = null;
        
        try
        {
            jsonValue = JsonConvert.SerializeObject(session);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to serialize to json value. Value: '{jsonValue}'", jsonValue);
            throw;
        }
        
        await _context.Database.StringSetAsync(BuildKey(session.Key), jsonValue);
    }

    public async Task RemoveAsync(string sessionKey)
    {
        bool deleted = await _context.Database.KeyDeleteAsync(BuildKey(sessionKey));

        if (!deleted)
        {
            _logger.LogWarning("Session is not deleted. SessionKey: '{sessionKey}'", sessionKey);
        }
    }

    private string BuildKey(string sessionKey) => "table.session_keys:" + sessionKey;
}