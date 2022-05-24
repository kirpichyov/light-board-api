using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public abstract class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey>
    where TEntity : EntityBase<TKey>
    where TKey : struct
{
    protected PostgreSqlContext Context;
    private readonly DbSet<TEntity> _table;

    protected RepositoryBase(PostgreSqlContext context)
    {
        Context = context;
        _table = Context.Set<TEntity>();
    }

    public void Add(TEntity entity)
    {
        _table.Add(entity);
    }

    public void Update(TEntity entity)
    {
        _table.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _table.Remove(entity);
    }

    public void Delete(TEntity[] entities)
    {
        _table.RemoveRange(entities);
    }
}