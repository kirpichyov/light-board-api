using LightBoard.Domain.Entities.Auth;

namespace LightBoard.Domain.Entities.Cards
{
    public class CardComment : EntityBase<Guid>
    {
        public CardComment(Guid cardId, Guid userId, string message)
            : base(Guid.NewGuid())
        {
            CardId = cardId;
            UserId = userId;
            Message = message;
            CreatedAtUtc = DateTime.UtcNow;
        }
        public CardComment()
        {

        }

        public Guid CardId;
        public Card Card;
        public Guid UserId;
        public User User;
        public string Message;
        public DateTime CreatedAtUtc;
    }
}
