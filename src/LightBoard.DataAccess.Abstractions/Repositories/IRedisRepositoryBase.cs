using LightBoard.Domain.Contracts;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IRedisRepositoryBase<TEntity, TKey> 
    where TEntity : class, IHasUniqueKey<TKey>
{
    Task<TEntity?> GetAsync(TKey key);
    Task AddAsync(TEntity entity);
    Task RemoveAsync(TKey key);
}