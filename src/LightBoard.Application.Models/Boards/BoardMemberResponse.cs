namespace LightBoard.Application.Models.Boards;

public class BoardMemberResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string UserAvatar { get; set; }
}