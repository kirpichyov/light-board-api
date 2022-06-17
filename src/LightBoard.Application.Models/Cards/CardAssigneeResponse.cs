namespace LightBoard.Application.Models.Cards;

public class CardAssigneeResponse
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public Guid UserId { get; set; }
}