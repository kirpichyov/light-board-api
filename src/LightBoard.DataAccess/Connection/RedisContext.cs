using LightBoard.DataAccess.Abstractions.Connection;
using StackExchange.Redis;

namespace LightBoard.DataAccess.Connection;

public class RedisContext : IRedisContext, IDisposable
{
    private readonly ConnectionMultiplexer _redisConnection;

    public IDatabase Database { get; }

    public RedisContext(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string can't be null or empty");
        }

        if (_redisConnection is not null)
        {
            throw new InvalidOperationException("Connection is already established");
        }

        _redisConnection = ConnectionMultiplexer.Connect(connectionString);
        Database = _redisConnection.GetDatabase();
    }

    public RedisKey[] GetKeys(string pattern)
    {
        var endPoint = _redisConnection.GetEndPoints().First();

        return _redisConnection
            .GetServer(endPoint)
            .Keys(pattern: pattern)
            .ToArray();
    }

    public void Dispose()
    {
        _redisConnection?.Close();
        _redisConnection?.Dispose();
    }
}