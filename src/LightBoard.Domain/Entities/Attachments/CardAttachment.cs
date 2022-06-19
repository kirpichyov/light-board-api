using LightBoard.Domain.Entities.Cards;

namespace LightBoard.Domain.Entities.Attachments
{
    public class CardAttachment : EntityBase<Guid>
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public DateTime UploadedAtUtc { get; set; } 
        public Guid CardId { get; set; }
        public Card Card { get; set; } 
    }
}
