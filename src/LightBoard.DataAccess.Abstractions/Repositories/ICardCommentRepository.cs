using LightBoard.Domain.Entities.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBoard.DataAccess.Abstractions.Repositories
{
    public interface ICardCommentRepository : IRelationalRepositoryBase<CardComment, Guid>
    {
        Task<IReadOnlyCollection<CardComment>> GetCommentsFromCard(Guid cardId);
        Task<CardComment> GetCommentById(Guid commentId);
    }
}
