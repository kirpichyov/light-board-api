using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Cards;
using LightBoard.DataAccess.Abstractions;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Shared.Exceptions;
using LightBoard.Domain.Entities.Attachments;

namespace LightBoard.Application.Services;

public class CardsService : ICardsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfoService _userInfo;
    private readonly IApplicationMapper _mapper;
    private readonly IBlobService _blobService;

    public CardsService(
        IUnitOfWork unitOfWork,
        IUserInfoService userInfo,
        IApplicationMapper mapper,
        IBlobService blobService)
    {
        _unitOfWork = unitOfWork;
        _userInfo = userInfo;
        _mapper = mapper;
        _blobService = blobService;
    }

    public async Task<CardResponse> UpdateCard(Guid id, UpdateCardRequest request)
    {
        var card = await _unitOfWork.Cards.GetForUser(id, _userInfo.UserId);

        card.Title = request.Title;
        card.Description = request.Description;

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToCardResponse(card);
    }

    public async Task DeleteCard(Guid id)
    {
        var card = await _unitOfWork.Cards.GetForUser(id, _userInfo.UserId);

        _unitOfWork.Cards.Delete(card);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<CardResponse> GetCard(Guid id)
    {
        var card = await _unitOfWork.Cards.GetForUser(id, _userInfo.UserId);

        return _mapper.ToCardResponse(card);
    }

    public async Task<CardResponse> UpdateOrder(Guid id, UpdateCardOrderRequest request)
    {
        var card = await _unitOfWork.Cards.GetForUser(id, _userInfo.UserId);

        var cardToSwap = card.Column.Cards.SingleOrDefault(columnCard => columnCard.Order == request.Order);

        if (cardToSwap is null)
        {
            card.Order = card.Column.Cards.Max(columnCard => columnCard.Order);

            foreach (var item in card.Column.Cards.Where(columnCard => columnCard.Id != card.Id))
            {
                item.Order--;
            }
        }
        else
        {
            var tempOrder = card.Order;

            card.Order = request.Order;

            cardToSwap.Order = tempOrder;
        }

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToCardResponse(card);
    }

    public async Task<CardAttachmentResponse> AddAttachments(Guid cardId, AddCardAttachmentRequest request)
    {
        var card = await _unitOfWork.Cards.GetForUser(cardId, _userInfo.UserId);

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

    public async Task<CardAssigneeResponse> AddAssigneeToCard(Guid id, AddAssigneeToCardRequest request)
    {
        var card = await _unitOfWork.Cards.GetForUser(id, _userInfo.UserId);

        if (!await _unitOfWork.Boards.HasAccessToBoard(card.Column.BoardId, request.UserId))
        {
            throw new NotFoundException("Board is not found");
        }

        if (card.CardAssignees.Any(cardAssignee => cardAssignee.UserId == request.UserId))
        {
            throw new ValidationFailedException("Card already has this assignee");
        }

        var cardAssignee = new CardAssignee(request.UserId, id);
        
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
}