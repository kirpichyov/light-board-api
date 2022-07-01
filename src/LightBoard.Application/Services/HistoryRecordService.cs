using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Paginations;
using LightBoard.Application.Models.Records;
using LightBoard.DataAccess.Abstractions;
using LightBoard.Domain.Entities.Record;
using LightBoard.Shared.Contracts;
using LightBoard.Shared.Exceptions;

namespace LightBoard.Application.Services;

public class HistoryRecordService : IHistoryRecordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationMapper _mapper;
    private readonly IUserInfoService _userInfoService;

    public HistoryRecordService(IUnitOfWork unitOfWork, IApplicationMapper mapper, IUserInfoService userInfoService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userInfoService = userInfoService;
    }

   public void AddHistoryRecord<TItem>(HistoryRecordArgs<TItem> historyRecordArgs)
     where TItem : IPureCloneable
   {
       _unitOfWork.HistoryRecords.Add(_mapper.ToActionHistoryRecord(historyRecordArgs));
   }

   public async Task<IReadOnlyCollection<HistoryRecordResponse>> GetAllHistoryRecord(Guid boardId, PaginationRequest paginationRequest)
   {
       var isHasAccess = await _unitOfWork.Boards.HasAccessToBoard(boardId, _userInfoService.UserId);
       if (!isHasAccess)
       {
           throw new NotFoundException("Board is not found.");
       }
       
       var historyRecords = await _unitOfWork.HistoryRecords.GetAll(boardId, paginationRequest.Take, paginationRequest.Skip);
       return _mapper.MapCollectionOrEmpty(historyRecords, _mapper.ToHistoryRecordResponse);
   }
}