using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Record;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class HistoryRecordRepository : RelationalRepositoryBase<ActionHistoryRecord, Guid>, IHistoryRecordRepository
{
    public HistoryRecordRepository(PostgreSqlContext context) 
        : base(context)
    {
    }

    public async Task<IReadOnlyCollection<ActionHistoryRecord>> GetAll(Guid boardId, int take, int skip)
    {
        return await Context.ActionHistoryRecords
            .Where(actionHistoryRecord => actionHistoryRecord.BoardId == boardId)
            .OrderByDescending(actionHistoryRecord => actionHistoryRecord.CreatedTime)
            .Skip(skip)
            .Take(take)
            .ToArrayAsync();
    }
}