using LightBoard.Application.Models.Auth;
using LightBoard.Application.Models.Users;

namespace LightBoard.Application.Abstractions.Services;

public interface IAuthService
{
    Task<(UserInfoResponse CreatedUserInfo, string SessionKey)> CreateUser(RegisterRequest request);
    Task<string> CreateUserSession(SignInRequest request);
    Task DeleteSession();
}