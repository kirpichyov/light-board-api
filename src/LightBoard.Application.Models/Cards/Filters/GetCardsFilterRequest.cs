using LightBoard.Shared.Models.Enums;

namespace LightBoard.Application.Models.Cards.Filters;

public class GetCardsFilterRequest
{
    public Guid[]? Assignees { get; set; }
    public SortingDirection? Direction { get; set; }
}