using LightBoard.Domain.Entities.Attachments;
using LightBoard.Domain.Entities.Columns;
using LightBoard.Domain.Enums;

namespace LightBoard.Domain.Entities.Cards;

public class Card : EntityBase<Guid>
{
    public Card(Guid columnId, string title, string description, DateTime? dedlineAtUtc, int order)
        : base(Guid.NewGuid())
    {
        ColumnId = columnId;
        Title = title;
        Description = description;
        DeadlineAtUtc = dedlineAtUtc;
        Order = order;
    }

    private Card()
    {
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public DateTime? DeadlineAtUtc { get; set; }
    public Guid ColumnId { get; }
    public Column Column { get; }
    public Priority Priority { get; set; }
    public ICollection<CardAssignee> CardAssignees { get; }
    public ICollection<CardAttachment> Attachments { get; set; }
}