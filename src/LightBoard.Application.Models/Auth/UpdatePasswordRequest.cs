using LightBoard.Application.Models.Auth.Internal;

namespace LightBoard.Application.Models.Auth;

public class UpdatePasswordRequest : IContainsPassword
{
    public string OldPassword { get; set; }
    public string Password { get; set; }
    public bool StaySignedIn { get; set; }
}