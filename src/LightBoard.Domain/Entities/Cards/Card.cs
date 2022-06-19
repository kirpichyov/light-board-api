using LightBoard.Domain.Entities.Attachments;
using LightBoard.Domain.Entities.Columns;

namespace LightBoard.Domain.Entities.Cards;

public class Card : EntityBase<Guid>
{
    public Card(Guid columnId, string title, string description, int order)
        : base(Guid.NewGuid())
    {
        ColumnId = columnId;
        Title = title;
        Description = description;
        Order = order;
    }

    private Card()
    {
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public Guid ColumnId { get; }
    public Column Column { get; }
    public ICollection<CardAssignee> CardAssignees { get; }
    public ICollection<CardAttachment> Attachments { get; set; }
}