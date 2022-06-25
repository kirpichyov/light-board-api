using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBoard.DataAccess.Repositories
{
    public class CardCommentRepository : RelationalRepositoryBase<CardComment, Guid>, ICardCommentRepository
    {
        public CardCommentRepository(PostgreSqlContext context)
            : base(context)
        {
        }

        public async Task<IReadOnlyCollection<CardComment>> GetCommentsFromCard(Guid cardId)
        {
            return await Context.CardComments.Where(comment => comment.CardId == cardId).ToArrayAsync();
        }

        public async Task<CardComment> GetCommentById(Guid commentId)
        {
            return await Context.CardComments.FirstOrDefaultAsync(comment => comment.Id == commentId) 
                ?? throw new NotFoundException("Comment not found");
        }
    }
}
