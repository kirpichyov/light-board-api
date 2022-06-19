using Microsoft.AspNetCore.Http;

namespace LightBoard.Application.Models.Cards
{
    public class CardAttachmentRequest
    {
        public IFormFile File { get; set; }
    }
}
