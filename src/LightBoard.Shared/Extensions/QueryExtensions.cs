using System.Linq.Expressions;
using LightBoard.Shared.Models.Enums;

namespace LightBoard.Shared.Extensions;

public static class QueryExtensions
{
    public static IQueryable<TEntity> SortBy<TEntity, TProperty>(
        this IQueryable<TEntity> queryable,
        Expression<Func<TEntity, TProperty>> propertySelector,
        SortingDirection? direction)
    {
        return direction switch
        {
            SortingDirection.Asc => queryable.OrderBy(propertySelector),
            SortingDirection.Desc => queryable.OrderByDescending(propertySelector),
            _ => queryable
        };
    }
}