using LightBoard.Application.Models.Enums;

namespace LightBoard.Application.Models.Cards;

public class UpdateCardRequest
{
    public string Title { get; set; }
    public string Description { get; set; } 
    public PriorityModel Priority { get; set; }
}