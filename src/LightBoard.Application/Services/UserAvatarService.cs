using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Results;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Shared.Exceptions;

namespace LightBoard.Application.Services
{
    public class UserAvatarService : IUserAvatarService
    {
        private readonly IKeysGenerator _keysGenerator;
        private readonly IBlobService _blobService;

        public UserAvatarService(IKeysGenerator keysGenerator, IBlobService blobService)
        {
            _keysGenerator = keysGenerator;
            _blobService = blobService;
        }

        public async Task<UploadBlobResult> GenerateUserAvatar()
        {
            var randomAvatarKey = _keysGenerator.Generate(10);
            using var client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync($"https://avatars.dicebear.com/api/gridy/{randomAvatarKey}.svg");

            if (!response.IsSuccessStatusCode)
            {
                throw new ExternalApiException("Avatars API call failed", await response.Content.ReadAsStringAsync());
            }

            var args = new UploadFileStreamArgs()
            {
                Container = BlobContainer.UserAvatars,
                Purpose = BlobPurpose.Inline,
                ContentStream = await response.Content.ReadAsStreamAsync(),
                ContentType = response.Content?.Headers?.ContentType?.MediaType ?? throw new ArgumentNullException("MediaType"),
                FileName = $"{randomAvatarKey}.svg"
            };

            var result = await _blobService.UploadStreamContent(args);

            return result;
        }
    }
}