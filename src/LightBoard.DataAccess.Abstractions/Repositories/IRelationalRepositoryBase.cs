using LightBoard.Domain.Entities;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IRelationalRepositoryBase<TEntity, TKey>
    where TEntity : EntityBase<TKey>
    where TKey : struct
{
    public void Add(TEntity entity);
    public void Update(TEntity entity);
    public void Delete(TEntity entity);
    public void Delete(TEntity[] entities);
}