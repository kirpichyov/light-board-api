using Microsoft.AspNetCore.Http;

namespace LightBoard.Application.Models.Cards
{
    public class AddCardAttachmentRequest
    {
        public IFormFile File { get; set; }
    }
}
