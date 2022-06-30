using LightBoard.Domain.Entities.Boards;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Shared.Contracts;

namespace LightBoard.Domain.Entities.Columns;

public class Column : EntityBase<Guid>, IPureCloneable
{
    public Column(string name, Guid boardId, int order) 
        : base(Guid.NewGuid())
    {
        Name = name;
        Order = order;
        BoardId = boardId;
    }
    
    public object GetPureObject()
    {
        return new 
        {
            Name = Name,
            Order = Order,
            BoardId = BoardId,
        };
    }

    private Column()
    {
    }

    public string Name { get; set; }
    public int Order { get; set; }
    public Guid BoardId { get; private set; }
    public Board Board { get; private set; }
    public ICollection<Card> Cards { get; }
}