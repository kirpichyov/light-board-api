using LightBoard.Domain.Entities.Record;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IHistoryRecordRepository : IRelationalRepositoryBase<ActionHistoryRecord, Guid>
{
    Task<IReadOnlyCollection<ActionHistoryRecord>> GetAll(Guid boardId, int take, int skip);
}