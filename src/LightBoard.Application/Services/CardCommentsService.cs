using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.CardComments;
using LightBoard.DataAccess.Abstractions;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBoard.Application.Services
{
    public class CardCommentsService : ICardCommentsService
    {
        IUnitOfWork _unitOfWork;
        IApplicationMapper _mapper;
        IUserInfoService _userInfoService;

        public CardCommentsService(
            IUnitOfWork unitOfWork, 
            IApplicationMapper mapper, 
            IUserInfoService userInfoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userInfoService = userInfoService;
        }

        public async Task<CommentResponse> CreateComment(Guid cardId, Guid userId, string message)
        {
            if(!await _unitOfWork.Cards.IsUserHasAccess(cardId, userId))
            {
                throw new AccessDeniedException("User have no access to this card");
            }

            var comment = new CardComment(cardId, userId, message);

            _unitOfWork.CardComments.Add(comment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.ToCommentResponse(comment);
        }

        public async Task<IReadOnlyCollection<CommentResponse>> GetComments(Guid cardId)
        {
            if (!await _unitOfWork.Cards.IsUserHasAccess(cardId, _userInfoService.UserId))
            {
                throw new AccessDeniedException("User have no access to this card");
            }

            return _mapper.MapCollection(await _unitOfWork.CardComments.GetCommentsFromCard(cardId), _mapper.ToCommentResponse);
        }

        public async Task<CommentResponse> UpdateComment(Guid commentId, UpdateCommentRequest request)
        {
            var comment = await _unitOfWork.CardComments.GetCommentById(commentId);

            if(_userInfoService.UserId != comment.UserId)
            {
                throw new AccessDeniedException("User have no access to edit this message");
            }

            comment.Message = request.Message;

            _unitOfWork.CardComments.Update(comment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.ToCommentResponse(comment);
        }

        public async Task DeleteComment(Guid commentId)
        {
            var comment = await _unitOfWork.CardComments.GetCommentById(commentId) ;
            if(comment.UserId != _userInfoService.UserId)
            {
                throw new AccessDeniedException("User have no access to delete this message");
            }

            _unitOfWork.CardComments.Delete(comment);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
