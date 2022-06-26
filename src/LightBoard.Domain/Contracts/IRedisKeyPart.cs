namespace LightBoard.Domain.Contracts;

public interface IRedisKeyPart<TKey>
{
    TKey RedisKeyPart { get; }
}