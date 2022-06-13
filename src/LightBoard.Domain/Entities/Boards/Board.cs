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
    public ICollection<BoardMember> BoardMembers { get; }
}