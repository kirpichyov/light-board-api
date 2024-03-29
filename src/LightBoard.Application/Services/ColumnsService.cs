﻿using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Cards;
using LightBoard.Application.Models.Columns;
using LightBoard.Application.Models.Records;
using LightBoard.DataAccess.Abstractions;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Domain.Entities.Columns;
using LightBoard.Shared.Extensions;
using LightBoard.Shared.Models.Enums;

namespace LightBoard.Application.Services;

public class ColumnsService : IColumnsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfoService _userInfo;
    private readonly IApplicationMapper _mapper;
    private readonly IHistoryRecordService _historyRecordService;
    private readonly IHtmlSanitizerService _htmlSanitizerService;

    public ColumnsService(
        IUnitOfWork unitOfWork, 
        IUserInfoService userInfo,
        IApplicationMapper mapper, 
        IHistoryRecordService historyRecordService,
        IHtmlSanitizerService htmlSanitizerService)
    {
        _unitOfWork = unitOfWork;
        _userInfo = userInfo;
        _mapper = mapper;
        _historyRecordService = historyRecordService;
        _htmlSanitizerService = htmlSanitizerService;
    }
    
    public async Task<ColumnResponse> UpdateColumn(Guid id, UpdateColumnNameRequest request)
    {
        var column = await _unitOfWork.Columns.GetColumnForUserById(id, _userInfo.UserId);
        
        var historyRecordsArgs = new HistoryRecordArgs<Column>
        {
            ActionType = ActionType.Create,
            CreatedTime = DateTime.UtcNow,
            ResourceId = column.Id,
            ResourceType = ResourceType.Column,
            UserId = _userInfo.UserId,
            BoardId = column.BoardId
        };

        historyRecordsArgs.SetOldValue(column);

        column.Name = request.Name;

        historyRecordsArgs.SetNewValue(column);

        _unitOfWork.Columns.Update(column);
        
        _historyRecordService.AddHistoryRecord(historyRecordsArgs);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToColumnResponse(column);
    }

    public async Task DeleteColumn(Guid id)
    {
        var column = await _unitOfWork.Columns.GetColumnForUserById(id, _userInfo.UserId);
        
        var historyRecordsArgs = new HistoryRecordArgs<Column>
        {
            ActionType = ActionType.Delete,
            CreatedTime = DateTime.UtcNow,
            ResourceId = column.Id,
            ResourceType = ResourceType.Column,
            UserId = _userInfo.UserId,
            BoardId = column.BoardId
        };
        
        historyRecordsArgs.SetOldValue(column);

        _unitOfWork.Columns.Delete(column);
        
        _historyRecordService.AddHistoryRecord(historyRecordsArgs);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ColumnResponse> GetColumn(Guid id)
    {
        var column = await _unitOfWork.Columns.GetColumnForUserById(id, _userInfo.UserId);

        return _mapper.ToColumnResponse(column);
    }

    public async Task<ColumnResponse> UpdateOrder(Guid id, UpdateColumnOrderRequest request)
    {
        var column = await _unitOfWork.Columns.GetColumnForUserById(id, _userInfo.UserId);
        
        var historyRecordsArgs = new HistoryRecordArgs<Column>
        {
            ActionType = ActionType.Update,
            CreatedTime = DateTime.UtcNow,
            ResourceId = column.Id,
            ResourceType = ResourceType.Column,
            UserId = _userInfo.UserId,
            BoardId = column.BoardId,
        };
        
        historyRecordsArgs.SetOldValue(column);

        var elementsCount = column.Board.Columns.MaxOrDefault(column => column.Order);
        
        column.Order = request.Order > elementsCount ? elementsCount : request.Order;
        
        if (request.Order > column.Order)
        {
            var collectionNewColumn = column.Board.Columns
                .Where(columnCard => columnCard.Order <= column.Order && columnCard.Id != column.Id)
                .ToArray();

            foreach (var item in collectionNewColumn)
            {
                item.Order--;
            }
        }
        else if (request.Order < column.Order)
        {
            var collectionNewColumn = column.Board.Columns
                .Where(columnCard => columnCard.Order >= column.Order && columnCard.Id != column.Id)
                .ToArray();

            foreach (var item in collectionNewColumn)
            {
                item.Order++;
            }
        }
        
        historyRecordsArgs.SetNewValue(column);
        
        _historyRecordService.AddHistoryRecord(historyRecordsArgs);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToColumnResponse(column);
    }

    public async Task<CardResponse> CreateCard(Guid id, CreateCardRequest request)
    {
        var column = await _unitOfWork.Columns.GetColumnForUserById(id, _userInfo.UserId);

        var card = new Card(column.Id, request.Title, request.Description, request.DeadlineAtUtc, column.Cards.Count + 1)
            {
                Priority = _mapper.ToPriority(request.Priority),
                Description = _htmlSanitizerService.Sanitize(request.Description)
            };

        var historyRecordsArgs = new HistoryRecordArgs<Card>
        {
            ActionType = ActionType.Create,
            CreatedTime = DateTime.UtcNow,
            ResourceId = card.Id,
            ResourceType = ResourceType.Card,
            UserId = _userInfo.UserId,
            BoardId = column.BoardId
        };
        
        historyRecordsArgs.SetNewValue(card);

        _unitOfWork.Cards.Add(card);
        
        _historyRecordService.AddHistoryRecord(historyRecordsArgs);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToCardResponse(card);
    }

    public async Task<IReadOnlyCollection<CardResponse>> GetColumnCards(Guid id)
    {
        var column = await _unitOfWork.Columns.GetColumnForUserById(id, _userInfo.UserId);

        return _mapper.MapCollection(column.Cards, _mapper.ToCardResponse);
    }
}