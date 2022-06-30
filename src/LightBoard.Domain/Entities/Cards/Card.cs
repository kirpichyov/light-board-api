using LightBoard.Domain.Entities.Attachments;
using LightBoard.Domain.Entities.Columns;
using LightBoard.Shared.Contracts;
using LightBoard.Domain.Enums;

namespace LightBoard.Domain.Entities.Cards;

public class Card : EntityBase<Guid>, IPureCloneable
{
    public Card(Guid columnId, string title, string description, DateTime? deadlineAtUtc, int order)
        : base(Guid.NewGuid())
    {
        ColumnId = columnId;
        Title = title;
        Description = description;
        DeadlineAtUtc = deadlineAtUtc;
        Order = order;
    }
    
    public object GetPureObject()
    {
        return new
        {
            Title = Title,
            Description = Description,
            Order = Order,
            DeadlineAtUtc = DeadlineAtUtc,
            Priority = Priority,
            ColumnId = ColumnId
        };
    }

    private Card()
    {
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public DateTime? DeadlineAtUtc { get; set; }
    public Guid ColumnId { get; private set; }
    public Column Column { get; private set; }
    public Priority Priority { get; set; }
    public ICollection<CardAssignee> CardAssignees { get; }
    public ICollection<CardAttachment> Attachments { get; set; }
    public ICollection<CardComment> Comments { get; set; }
}