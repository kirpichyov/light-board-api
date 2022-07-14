namespace LightBoard.Application.Models.Cards;

public class ChangeCardColumnRequest
{
    public Guid ColumnId { get; set; }
    
    public int Order { get; set; }
}