using LightBoard.Api.Swagger.Models;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Auth;
using LightBoard.Application.Models.Users;
using LightBoard.Shared.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LightBoard.Api.Controllers;

public class AuthController : ApiControllerBase
{
    private readonly IAuthService _authService;
    private readonly IProfileService _profileService;

    public AuthController(IAuthService authService, IProfileService profileService)
    {
        _authService = authService;
        _profileService = profileService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserInfoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var creationResult = await _authService.CreateUser(request);
        
        Response.Headers.Add(ApiHeaders.SessionKey, creationResult.SessionKey);
        return new ObjectResult(creationResult.CreatedUserInfo);
    }

    [HttpPut("password")]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
    {
        await _profileService.UpdatePassword(request);
        return NoContent();
    }
    
    [HttpPost("request-password-reset-email")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RequestPasswordReset([FromBody] ResetPasswordEmailRequest request)
    {
        await _profileService.RequestPasswordReset(request);
        return NoContent();
    }
    
    [HttpPost("confirm-email")]
    [ProducesResponseType(typeof(UpdateAvatarResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmail([FromForm] string confirmEmailCode)
    {
        await _profileService.ConfirmEmail(confirmEmailCode);
        return NoContent();
    }
    
    [HttpPost("password-reset")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        await _profileService.ResetPassword(request);
        return NoContent();
    }
    
    [HttpPost("sign-in")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        var sessionKey = await _authService.CreateUserSession(request);
        
        Response.Headers.Add(ApiHeaders.SessionKey, sessionKey);
        return NoContent();
    }
    
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Logout()
    {
        await _authService.DeleteSession();
        return NoContent();
    }
}