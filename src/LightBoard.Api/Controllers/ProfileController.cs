using LightBoard.Api.Swagger.Models;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace LightBoard.Api.Controllers
{
    public class ProfileController : ApiControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost("request-confirmation-email")]
        [ProducesResponseType(typeof(UpdateAvatarResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RequestConfirmEmail()
        {
            await _profileService.RequestEmailConfirmation();
            return NoContent();
        }

        [HttpPut("avatar")]
        [ProducesResponseType(typeof(UpdateAvatarResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        public async Task<UpdateAvatarResponse> UpdateAvatar([FromForm] UpdateAvatarRequest request)
        {
            return await _profileService.UpdateAvatar(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
        public async Task<UserProfileResponse> GetProfile()
        {
            return await _profileService.GetProfile();
        }
    }
}
