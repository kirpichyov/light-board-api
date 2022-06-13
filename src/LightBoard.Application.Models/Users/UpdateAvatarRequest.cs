using Microsoft.AspNetCore.Http;

namespace LightBoard.Application.Models.Users
{
    public class UpdateAvatarRequest
    {
        public IFormFile File { get; set; }
    }
}
