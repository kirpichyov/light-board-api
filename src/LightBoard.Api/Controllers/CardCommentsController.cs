using LightBoard.Api.Swagger.Models;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.CardComments;
using Microsoft.AspNetCore.Mvc;

namespace LightBoard.Api.Controllers
{
    public class CardCommentsController : ApiControllerBase
    {
        private readonly ICardCommentsService _cardCommentsService;

        public CardCommentsController(ICardCommentsService cardCommentsService)
        {
            _cardCommentsService = cardCommentsService;
        }

        [HttpPost("{commentId:guid}")]
        [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        public async Task<CommentResponse> UpdateComment([FromRoute] Guid commentId, [FromBody] UpdateCommentRequest request)
        {
            return await _cardCommentsService.UpdateComment(commentId, request);
        }

        [HttpDelete("{commentId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
        {
            await _cardCommentsService.DeleteComment(commentId);
            return NoContent();
        }
    }
}
