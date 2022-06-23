using LightBoard.DataAccess.Abstractions.Connection;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.Domain.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LightBoard.DataAccess.Repositories;

public abstract class RedisRepositoryBase<TEntity, TKey> : IRedisRepositoryBase<TEntity, TKey>
    where TEntity : class, IHasUniqueKey<TKey>
{
    private readonly IRedisContext _context;
    private readonly ILogger<RedisRepositoryBase<TEntity, TKey>> _logger;

    protected RedisRepositoryBase(IRedisContext context, ILogger<RedisRepositoryBase<TEntity, TKey>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TEntity?> GetAsync(TKey key)
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

    public async Task AddAsync(TEntity entity)
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
        
        await _context.Database.StringSetAsync(GenerateRedisKey(entity.UniqueKey), jsonValue);
    }

    public async Task RemoveAsync(TKey key)
    {
        var redisKey = GenerateRedisKey(key);
        var isDeleted = await _context.Database.KeyDeleteAsync(redisKey);

        if (!isDeleted)
        {
            _logger.LogWarning("Entity is not deleted. Redis key: '{redisKey}'", redisKey);
        }
    }

    protected abstract string GenerateRedisKey(TKey key);
}