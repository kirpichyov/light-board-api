using LightBoard.Application.Models.Auth;
using LightBoard.Application.Models.Users;

namespace LightBoard.Application.Abstractions.Services;

public interface IPasswordService
{
    Task UpdatePassword(UpdatePasswordRequest request);

    Task RequestPasswordReset(ResetPasswordEmailRequest request);

    Task ResetPassword(ResetPasswordRequest request);
}