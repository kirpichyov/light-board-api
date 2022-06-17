using LightBoard.Domain.Entities.Auth;

namespace LightBoard.Domain.Entities.Cards;

public class CardAssignee : EntityBase<Guid>
{
    public CardAssignee(Guid userId, Guid cardId)
        : base(Guid.NewGuid())
    {
        UserId = userId;
        CardId = cardId;
    }

    private CardAssignee()
    {
    }

    public Guid UserId { get; }
    public Guid CardId { get; }
    public User User { get; }
    public Card Card { get; }
}