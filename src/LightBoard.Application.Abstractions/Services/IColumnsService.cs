using LightBoard.Application.Models.Cards;
using LightBoard.Application.Models.Columns;

namespace LightBoard.Application.Abstractions.Services;

public interface IColumnsService
{
    Task<ColumnResponse> UpdateColumn(Guid id, UpdateColumnNameRequest request);
    Task DeleteColumn(Guid id);
    Task<ColumnResponse> GetColumn(Guid id);
    Task<ColumnResponse> UpdateOrder(Guid id, UpdateColumnOrderRequest request);
    Task<CardResponse> CreateCard(Guid id, CreateCardRequest request);
    Task<IReadOnlyCollection<CardResponse>> GetColumnCards(Guid id);
}