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
    private readonly IPasswordService _passwordService;

    public AuthController(IAuthService authService, IPasswordService passwordService)
    {
        _authService = authService;
        _passwordService = passwordService;
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
        await _passwordService.UpdatePassword(request);
        return NoContent();
    }
    
    [HttpPost("request-password-reset-email")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RequestPasswordReset([FromBody] ResetPasswordEmailRequest request)
    {
        await _passwordService.RequestPasswordReset(request);
        return NoContent();
    }
    
    [HttpPost("password-reset")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        await _passwordService.ResetPassword(request);
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