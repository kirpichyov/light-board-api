using LightBoard.Application.Models.Paginations;
using LightBoard.Application.Models.Records;
using LightBoard.Shared.Contracts;

namespace LightBoard.Application.Abstractions.Services;

public interface IHistoryRecordService
{
    void AddHistoryRecord<TItem>(HistoryRecordArgs<TItem> historyRecordArgs)
        where TItem : IPureCloneable;
    Task<IReadOnlyCollection<HistoryRecordResponse>> GetAllHistoryRecord(Guid boardId, PaginationRequest paginationRequest);
}