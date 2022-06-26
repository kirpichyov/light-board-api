using LightBoard.Domain.Contracts;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IRedisRepositoryBase<TEntity, TKey> 
    where TEntity : class, IRedisKeyPart<TKey>
{
    Task<TEntity?> GetAsync(TKey key);
    Task<string> AddAsync(TEntity entity, TimeSpan? lifetime = null);
    Task RemoveAsync(TKey identifier);
}