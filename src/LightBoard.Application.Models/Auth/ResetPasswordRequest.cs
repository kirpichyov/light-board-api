using LightBoard.Application.Models.Auth.Internal;

namespace LightBoard.Application.Models.Auth;

public class ResetPasswordRequest : IContainsPassword
{
    public string Password { get; set; }
    public string ResetCode { get; set; }
}