using LightBoard.Application.Models.Auth.Internal;

namespace LightBoard.Application.Models.Auth;

public class RegisterRequest : IContainsPassword
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
}