using LightBoard.Application.Models.Auth;
using LightBoard.Application.Models.Users;

namespace LightBoard.Application.Abstractions.Services
{
    public interface IProfileService
    {
        Task<UpdateAvatarResponse> UpdateAvatar(UpdateAvatarRequest request);
        
        Task<UserProfileResponse> GetProfile();
        
        Task UpdatePassword(UpdatePasswordRequest request);

        Task RequestPasswordReset(ResetPasswordEmailRequest request);

        Task ResetPassword(ResetPasswordRequest request);

        Task RequestEmailConfirmation();
        
        Task ConfirmEmail(string confirmEmailCode);
    }
}
