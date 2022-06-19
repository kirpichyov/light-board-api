namespace LightBoard.Application.Abstractions.Services
{
    public interface ICardAttachmentService
    {
        Task DeleteCardAttachment(Guid attachmentId);
    }
}
