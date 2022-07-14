using LightBoard.Domain.Entities.Columns;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IColumnsRepository : IRelationalRepositoryBase<Column, Guid>
{
    Task<Column> GetColumnForUserById(Guid columnId, Guid userId);
}