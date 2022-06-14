using LightBoard.Domain.Entities.Boards;

namespace LightBoard.Domain.Entities.Columns;

public class Column : EntityBase<Guid>
{
    public Column(string name, Guid boardId, int order) 
        : base(Guid.NewGuid())
    {
        Name = name;
        Order = order;
        BoardId = boardId;
    }

    private Column()
    {
    }

    public string Name { get; set; }
    public int Order { get; set; }
    public Guid BoardId { get; }
    public Board Board { get; }
}