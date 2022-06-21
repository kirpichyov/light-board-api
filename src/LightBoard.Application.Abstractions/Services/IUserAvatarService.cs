using LightBoard.Application.Abstractions.Results;


namespace LightBoard.Application.Abstractions.Services
{
    public interface IUserAvatarService
    {
        Task<UploadBlobResult> GenerateUserAvatar();
    }
}
