using LightBoard.Domain.Entities.Attachments;

namespace LightBoard.DataAccess.Abstractions.Repositories
{
    public interface IAttachmentRepository : IRelationalRepositoryBase<CardAttachment, Guid>
    {
        Task<CardAttachment> GetForUser(Guid id, Guid userId);
    }
}
