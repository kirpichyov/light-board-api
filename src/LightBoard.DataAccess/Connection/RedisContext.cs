using LightBoard.DataAccess.Abstractions.Connection;
using StackExchange.Redis;

namespace LightBoard.DataAccess.Connection;

public class RedisContext : IRedisContext, IDisposable
{
    private readonly ConnectionMultiplexer RedisConnection;

    public IDatabase Database { get; }
    
    public RedisContext(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string can't be null or empty");
        }

        if (RedisConnection is not null)
        {
            throw new InvalidOperationException("Connection is already established");
        }

        RedisConnection = ConnectionMultiplexer.Connect(connectionString);
        Database = RedisConnection.GetDatabase();
    }

    public void Dispose()
    {
        RedisConnection?.Close();
        RedisConnection?.Dispose();
    }
}