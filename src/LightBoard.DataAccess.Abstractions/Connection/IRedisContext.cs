using StackExchange.Redis;

namespace LightBoard.DataAccess.Abstractions.Connection;

public interface IRedisContext
{
    IDatabase Database { get; }
    RedisKey[] GetKeys(string pattern);
}