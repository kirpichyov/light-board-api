using LightBoard.Application.Models.Enums;

namespace LightBoard.Application.Models.Cards;

public class CardResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public PriorityModel Priority { get; set; }
    public IReadOnlyCollection<AssigneeResponse> Assignees { get; set; }
    public IReadOnlyCollection<CardAttachmentResponse> Attachments { get; set; }
}