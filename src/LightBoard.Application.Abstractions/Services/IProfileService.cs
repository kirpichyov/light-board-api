using LightBoard.Application.Models.Users;

namespace LightBoard.Application.Abstractions.Services
{
    public interface IProfileService
    {
        public Task<UpdateAvatarResponse> UpdateAvatar(UpdateAvatarRequest request);
        public Task<UserProfileResponse> GetProfile();
    }
}
