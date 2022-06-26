using LightBoard.DataAccess.Abstractions.Connection;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.Domain.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LightBoard.DataAccess.Repositories;

public abstract class RedisRepositoryBase<TEntity, TKeyPart> : IRedisRepositoryBase<TEntity, TKeyPart>
    where TEntity : class, IRedisKeyPart<TKeyPart>
{
    private readonly IRedisContext _context;
    private readonly ILogger<RedisRepositoryBase<TEntity, TKeyPart>> _logger;

    protected RedisRepositoryBase(IRedisContext context, ILogger<RedisRepositoryBase<TEntity, TKeyPart>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TEntity?> GetAsync(TKeyPart key)
    {
        var redisValue = await _context.Database.StringGetAsync(GenerateRedisKey(key));

        if (!redisValue.HasValue)
        {
            return null;
        }

        var jsonValue = redisValue.ToString();

        try
        {
            return JsonConvert.DeserializeObject<TEntity>(jsonValue);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to deserialize redis json value. Value: '{jsonValue}'", jsonValue);
            throw;
        }
    }

    public async Task<string> AddAsync(TEntity entity, TimeSpan? lifetime = null)
    {
        string? jsonValue = null;
        
        try
        {
            jsonValue = JsonConvert.SerializeObject(entity);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to serialize to json value. Value: '{jsonValue}'", jsonValue);
            throw;
        }

        var redisKey = GenerateRedisKey(entity.RedisKeyPart);
        await _context.Database.StringSetAsync(redisKey, jsonValue, lifetime);

        return redisKey;
    }

    public async Task RemoveAsync(TKeyPart identifier)
    {
        var redisKey = GenerateRedisKey(identifier);
        var isDeleted = await _context.Database.KeyDeleteAsync(redisKey);

        if (!isDeleted)
        {
            _logger.LogWarning("Entity is not deleted. Redis key: '{redisKey}'", redisKey);
        }
    }

    protected abstract string GenerateRedisKey(TKeyPart identifier);
}