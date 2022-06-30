using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Paginations;
using LightBoard.Application.Models.Records;
using LightBoard.DataAccess.Abstractions;
using LightBoard.Domain.Entities.Record;
using LightBoard.Shared.Contracts;

namespace LightBoard.Application.Services;

public class HistoryRecordService : IHistoryRecordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationMapper _mapper;

    public HistoryRecordService(IUnitOfWork unitOfWork, IApplicationMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

   public void AddHistoryRecord<TItem>(HistoryRecordArgs<TItem> historyRecordArgs)
     where TItem : IPureCloneable
   {
       _unitOfWork.HistoryRecords.Add(_mapper.ToActionHistoryRecord(historyRecordArgs));
   }

   public async Task<IReadOnlyCollection<HistoryRecordResponse>> GetAllHistoryRecord(Guid boardId, PaginationRequest paginationRequest)
   {
       var historyRecords =
           await _unitOfWork.HistoryRecords.GetAll(boardId, paginationRequest.Take, paginationRequest.Skip);
       return _mapper.MapCollectionOrEmpty(historyRecords, _mapper.ToHistoryRecordResponse);
   }
}