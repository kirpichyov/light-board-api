namespace LightBoard.Application.Models.Auth;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
}