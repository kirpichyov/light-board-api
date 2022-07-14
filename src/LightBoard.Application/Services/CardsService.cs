using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Cards;
using LightBoard.Application.Models.Records;
using LightBoard.DataAccess.Abstractions;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Shared.Exceptions;
using LightBoard.Domain.Entities.Attachments;
using LightBoard.Shared.Extensions;
using LightBoard.Shared.Models.Enums;

namespace LightBoard.Application.Services;

public class CardsService : ICardsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfoService _userInfo;
    private readonly IHistoryRecordService _historyRecordService;
    private readonly IApplicationMapper _mapper;
    private readonly IBlobService _blobService;

    public CardsService(
        IUnitOfWork unitOfWork,
        IUserInfoService userInfo,
        IApplicationMapper mapper,
        IBlobService blobService, 
        IHistoryRecordService historyRecordService)
    {
        _unitOfWork = unitOfWork;
        _userInfo = userInfo;
        _mapper = mapper;
        _blobService = blobService;
        _historyRecordService = historyRecordService;
    }

    public async Task<CardResponse> UpdateCard(Guid cardId, UpdateCardRequest request)
    {
        var card = await _unitOfWork.Cards.GetCardForUserById(cardId, _userInfo.UserId);
        
        var historyRecordsArgs = new HistoryRecordArgs<Card>
        {
            ActionType = ActionType.Update,
            CreatedTime = DateTime.UtcNow,
            ResourceId = card.Id,
            ResourceType = ResourceType.Card,
            UserId = _userInfo.UserId,
            BoardId = card.Column.BoardId,
        };
        
        historyRecordsArgs.SetOldValue(card);

        card.Title = request.Title;
        card.Description = request.Description;
        card.DeadlineAtUtc = request.DeadlineAtUtc;
        card.Priority = _mapper.ToPriority(request.Priority);

        historyRecordsArgs.SetNewValue(card);
        
        _historyRecordService.AddHistoryRecord(historyRecordsArgs);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToCardResponse(card);
    }

    public async Task DeleteCard(Guid cardId)
    {
        var card = await _unitOfWork.Cards.GetCardForUserById(cardId, _userInfo.UserId);
        
        var historyRecordsArgs = new HistoryRecordArgs<Card>
        {
            ActionType = ActionType.Delete,
            CreatedTime = DateTime.UtcNow,
            ResourceId = card.Id,
            ResourceType = ResourceType.Card,
            UserId = _userInfo.UserId,
            BoardId = card.Column.BoardId,
        };
        
        historyRecordsArgs.SetOldValue(card);

        _unitOfWork.Cards.Delete(card);
        
        _historyRecordService.AddHistoryRecord(historyRecordsArgs);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<CardResponse> GetCard(Guid cardId)
    {
        var card = await _unitOfWork.Cards.GetCardForUserById(cardId, _userInfo.UserId);

        return _mapper.ToCardResponse(card);
    }

    public async Task<CardResponse> UpdateOrder(Guid cardId, UpdateCardOrderRequest request)
    {
        var card = await _unitOfWork.Cards.GetCardForUserById(cardId, _userInfo.UserId);
        
        var historyRecordsArgs = new HistoryRecordArgs<Card>
        {
            ActionType = ActionType.Update,
            CreatedTime = DateTime.UtcNow,
            ResourceId = card.Id,
            ResourceType = ResourceType.Card,
            UserId = _userInfo.UserId,
            BoardId = card.Column.BoardId
        };
        
        historyRecordsArgs.SetOldValue(card);
        
        var elementsCount = card.Column.Cards.MaxOrDefault(column => column.Order);

        card.Order = request.Order > elementsCount ? elementsCount : request.Order;
        
        if (request.Order > card.Order)
        {
            var collectionNewColumn = card.Column.Cards
                .Where(columnCard => columnCard.Order <= card.Order && columnCard.Id != card.Id)
                .ToArray();

            foreach (var item in collectionNewColumn)
            {
                item.Order--;
            }
        }
        else if (request.Order < card.Order)
        {
            var collectionNewColumn = card.Column.Cards
                .Where(columnCard => columnCard.Order >= card.Order && columnCard.Id != card.Id)
                .ToArray();

            foreach (var item in collectionNewColumn)
            {
                item.Order++;
            }
        }

        historyRecordsArgs.SetNewValue(card);
        
        _historyRecordService.AddHistoryRecord(historyRecordsArgs);
        
        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToCardResponse(card);
    }

    public async Task<CardAttachmentResponse> AddAttachment(Guid cardId, AddCardAttachmentRequest request)
    {
        var card = await _unitOfWork.Cards.GetCardForUserById(cardId, _userInfo.UserId);

        var args = new UploadFormFileArgs()
        {
            Container = BlobContainer.CardAttachments,
            Purpose = BlobPurpose.Attachment,
            FormFile = request.File
        };

        var result = await _blobService.UploadFormFile(args);

        var attachment = new CardAttachment()
        {
            Name = request.File.FileName,
            Url = result.Uri,
            UploadedAtUtc = DateTime.UtcNow,
            CardId = card.Id  
        };

        _unitOfWork.Attachments.Add(attachment);

        await _unitOfWork.SaveChangesAsync();
        return _mapper.ToCardAttachmentResponse(attachment);
    }

    public async Task<CardAssigneeResponse> AddAssigneeToCard(Guid cardId, AddAssigneeToCardRequest request)
    {
        var card = await _unitOfWork.Cards.GetCardForUserById(cardId, _userInfo.UserId);

        if (!await _unitOfWork.Boards.HasAccessToBoard(card.Column.BoardId, request.UserId))
        {
            throw new NotFoundException("Board is not found");
        }

        if (card.CardAssignees.Any(cardAssignee => cardAssignee.UserId == request.UserId))
        {
            throw new ValidationFailedException("Card already has this assignee");
        }

        var cardAssignee = new CardAssignee(request.UserId, cardId);
        
        _unitOfWork.CardAssignees.Add(cardAssignee);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToCardAssigneeResponse(cardAssignee);
    }

    public async Task DeleteAssigneeFromCard(Guid cardAssigneeId)
    {
        var cardAssignee = await _unitOfWork.CardAssignees.GetById(cardAssigneeId);
        
        if (cardAssignee is null)
        {
            throw new NotFoundException("Assignee not found");
        }
        
        _unitOfWork.CardAssignees.Delete(cardAssignee);
        
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<CardResponse> ChangeCardColumn(Guid cardId, ChangeCardColumnRequest request)
    {
        var card = await _unitOfWork.Cards.GetCardForUserById(cardId, _userInfo.UserId);
        
        var historyRecordsArgs = new HistoryRecordArgs<Card>
        {
            ActionType = ActionType.Update,
            CreatedTime = DateTime.UtcNow,
            ResourceId = card.Id,
            ResourceType = ResourceType.Card,
            UserId = _userInfo.UserId,
            BoardId = card.Column.BoardId,
        };
        
        historyRecordsArgs.SetOldValue(card);

        var columnDestination = await _unitOfWork.Columns.GetColumnForUserById(request.ColumnId, _userInfo.UserId);

        if (card.Column.BoardId != columnDestination.BoardId)
        {
            throw new NotFoundException($"Column with id: {columnDestination.Id} has not found in the same board with card.");
        }

        if (card.ColumnId == request.ColumnId)
        {
            throw new ValidationFailedException("Card is already in the requested column.");
        }

        var oldColumn = card.Column;
        var newColumn = columnDestination;
        
        // Update column id
        card.ColumnId = request.ColumnId;
        
        // Update orders in old column
        var collectionOldColumn = oldColumn.Cards.Where(columnCard => columnCard.Order > card.Order).ToArray();

        foreach (var item in collectionOldColumn)
        {
            item.Order--;
        }
        
        // Determine card order in new column
        card.Order = newColumn.Cards.MaxOrDefault(card => card.Order, 0) + 1;

        var cardToSwap = newColumn.Cards.SingleOrDefault(columnCard => columnCard.Order == request.Order);
            
        if (cardToSwap is null)
        {
            var collectionNewColumn = newColumn.Cards.Where(columnCard => columnCard.Order > card.Order).ToArray();

            foreach (var item in collectionNewColumn)
            {
                item.Order--;
            }
        }
        else
        {
            card.Order = request.Order;
            
            var collectionNewColumn = newColumn.Cards.Where(columnCard => columnCard.Order >= card.Order).ToArray();

            foreach (var item in collectionNewColumn)
            {
                item.Order++;
            }
        }

        historyRecordsArgs.SetNewValue(card);
        
        _unitOfWork.Cards.Update(card);
        
        _historyRecordService.AddHistoryRecord(historyRecordsArgs);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToCardResponse(card);
    }
}