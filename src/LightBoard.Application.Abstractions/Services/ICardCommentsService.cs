using LightBoard.Application.Models.CardComments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBoard.Application.Abstractions.Services
{
    public interface ICardCommentsService
    {
        Task<CommentResponse> CreateComment(Guid cardId, string message);
        Task<IReadOnlyCollection<CommentResponse>> GetComments(Guid cardId);
        Task<CommentResponse> UpdateComment(Guid commentId, UpdateCommentRequest request);
        Task DeleteComment(Guid commentId);
    }
}
