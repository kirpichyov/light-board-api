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
                // TODO: Implement in https://trello.com/c/XI3t8c0H/51-be-link-action-history-items-to-boards
            //.Where(actionHistoryRecord => actionHistoryRecord.BoardId == boardId)
            .OrderByDescending(actionHistoryRecord => actionHistoryRecord.CreatedTime)
            .Skip(skip)
            .Take(take)
            .ToArrayAsync();
    }
}