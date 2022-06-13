using LightBoard.Domain.Entities.Auth;

namespace LightBoard.Domain.Entities.Boards;

public class BoardMember : EntityBase<Guid>
{
    public BoardMember(Guid userId, Guid boardId)
        : base(Guid.NewGuid())
    {
        UserId = userId;
        BoardId = boardId;
    }

    private BoardMember() 
    {
    }

    public Guid UserId { get; }
    public Guid BoardId { get; }
    public Board Board { get; }
    public User User { get;  }
}