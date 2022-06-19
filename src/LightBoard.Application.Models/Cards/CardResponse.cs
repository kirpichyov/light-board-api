namespace LightBoard.Application.Models.Cards;

public class CardResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public IReadOnlyCollection<AssigneeResponse> Assignees { get; set; }
    public IReadOnlyCollection<AttachmentResponse> Attachments { get; set; }
}