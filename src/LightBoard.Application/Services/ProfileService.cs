using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Users;
using LightBoard.DataAccess.Abstractions;

namespace LightBoard.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationMapper _mapper;
        private readonly IBlobService _blobService;
        private readonly IUserInfoService _userInfoService;

        public ProfileService(IUnitOfWork unitOfWork, IApplicationMapper applicationMapper, IBlobService blobService, IUserInfoService userInfoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = applicationMapper;
            _blobService = blobService;
            _userInfoService = userInfoService;
        }

        public async Task<UpdateAvatarResponse> UpdateAvatar(UpdateAvatarRequest request)
        {
            var args = new UploadBlobArgs()
            {
                 BlobContainer = BlobContainer.UserAvatars,
                 BlobPurpose = BlobPurpose.Inline,
                 FormFile = request.File
            };
            var result = await _blobService.UploadBlob(args);
            
            var userId = _userInfoService.UserId;
            var user = await _unitOfWork.Users.GetById(userId);

            if (user.AvatarBlobName != null)
            {
                await _blobService.DeleteFile(BlobContainer.UserAvatars, user.AvatarBlobName);
            }
            user.AvatarUrl = result.Uri;
            user.AvatarBlobName = result.BlobName;
            await _unitOfWork.SaveChangesAsync();
            return new UpdateAvatarResponse() 
            {
                UserAvatarUrl = user.AvatarUrl
            };
        }

        public async Task<UserProfileResponse> GetProfile()
        {
            var userId = _userInfoService.UserId;
            var user = await _unitOfWork.Users.GetById(userId);
             
            return _mapper.ToUserProfileResponse(user);
        }

    }
}
