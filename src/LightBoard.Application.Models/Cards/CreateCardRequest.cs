using LightBoard.Application.Models.Enums;

namespace LightBoard.Application.Models.Cards;

public class CreateCardRequest
{
    public string Title { get; set; }
    public string Description { get; set; } 
    public PriorityModel Priority { get; set; } 
    public DateTime? DeadlineAtUtc { get; set; }
}