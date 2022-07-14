using LightBoard.Application.Models.Cards;

namespace LightBoard.Application.Abstractions.Services;

public interface ICardsService
{
    Task<CardResponse> UpdateCard(Guid cardId, UpdateCardRequest request);
    Task DeleteCard(Guid cardId);
    Task<CardResponse> GetCard(Guid cardId);
    Task<CardResponse> UpdateOrder(Guid cardId, UpdateCardOrderRequest request);
    Task<CardResponse> ChangeCardColumn(Guid cardId, ChangeCardColumnRequest request);
    Task<CardAssigneeResponse> AddAssigneeToCard(Guid cardId, AddAssigneeToCardRequest request);
    Task DeleteAssigneeFromCard(Guid cardAssigneeId);
    Task<CardAttachmentResponse> AddAttachment(Guid cardId, AddCardAttachmentRequest request);
}