using Microsoft.AspNetCore.Http;

namespace LightBoard.Application.Models.Boards;

public class CreateBoardRequest
{
    public string Name { get; set; }
    public IFormFile? BoardBackground { get; set; }
}