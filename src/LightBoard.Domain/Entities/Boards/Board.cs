using LightBoard.Domain.Entities.Columns;
using LightBoard.Shared.Contracts;

namespace LightBoard.Domain.Entities.Boards;

public class Board : EntityBase<Guid>, IPureCloneable
{
    public Board(string name)
        : base(Guid.NewGuid())
    {
        Name = name;
        IsArchived = false;
    }
    
    public object GetPureObject()
    {
        return new
        {
            Name = Name,
            IsArchived = IsArchived,
            BackgroundUrl = BackgroundUrl,
            BackgroundBlobName = BackgroundBlobName
        };
    }

    private Board()
    {
    }

    public string Name { get; set; }
    public bool IsArchived { get; set; }
    public string BackgroundUrl { get; set; }
    public string BackgroundBlobName { get; set; }
    public ICollection<BoardMember> BoardMembers { get; }
    public ICollection<Column> Columns { get; }
}