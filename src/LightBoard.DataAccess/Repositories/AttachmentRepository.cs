using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Attachments;
using LightBoard.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories
{
    public class AttachmentRepository : RelationalRepositoryBase<CardAttachment, Guid>, IAttachmentRepository
    {
        public AttachmentRepository(PostgreSqlContext context)
            : base(context)
        {
        }

        public async Task<CardAttachment> GetForUser(Guid id, Guid userId)
        {
            return await Context.CardAttachment.Include(card => card.Card)
                       .ThenInclude(card => card.Column)
                       .ThenInclude(column => column.Cards)
                       .SingleOrDefaultAsync(attachment => attachment.Id == id &&
                                                     attachment.Card.Column.Board.BoardMembers.Any(member => member.UserId == userId))
                   ?? throw new NotFoundException("Card attachment is not found");
        }
    }
}
