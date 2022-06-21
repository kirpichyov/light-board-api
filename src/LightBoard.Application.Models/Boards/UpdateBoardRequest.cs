using Microsoft.AspNetCore.Http;

namespace LightBoard.Application.Models.Boards;

public class UpdateBoardRequest
{
    public string? Name { get; set; }
    public IFormFile? Background { get; set; }
}