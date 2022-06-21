using LightBoard.Domain.Entities.Columns;

namespace LightBoard.Domain.Entities.Boards;

public class Board : EntityBase<Guid>
{
    public Board(string name)
        : base(Guid.NewGuid())
    {
        Name = name;
        IsArchived = false;
    }

    private Board()
    {
    }

    public string Name { get; set; }
    public bool IsArchived { get; set; }
    public string BackgroundUrl { get; set; }
    public ICollection<BoardMember> BoardMembers { get; }
    public ICollection<Column> Columns { get; }
}