using LightBoard.Api.Swagger.Models;
using LightBoard.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LightBoard.Api.Controllers
{
    public class AttachmentsController : ApiControllerBase
    {
        private readonly ICardAttachmentService _attachmentService;

        public AttachmentsController(ICardAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpDelete("{attachmentId:guid}")]
        [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCardAttachment([FromRoute] Guid attachmentId)
        {
            await _attachmentService.DeleteCardAttachment(attachmentId);

            return NoContent();
        }
    }
}
