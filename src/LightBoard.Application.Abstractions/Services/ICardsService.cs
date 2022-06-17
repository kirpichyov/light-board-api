using LightBoard.Application.Models.Cards;

namespace LightBoard.Application.Abstractions.Services;

public interface ICardsService
{
    Task<CardResponse> UpdateCard(Guid id, UpdateCardRequest request);
    Task DeleteCard(Guid id);
    Task<CardResponse> GetCard(Guid id);
    Task<CardResponse> UpdateOrder(Guid id, UpdateCardOrderRequest request);
    Task<CardAssigneeResponse> AddAssigneeToCard(Guid id, AddAssigneeToCardRequest request);
    Task DeleteAssigneeFromCard(Guid cardAssigneeId);
}