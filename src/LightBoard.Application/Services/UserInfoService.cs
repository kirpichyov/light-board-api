using LightBoard.Application.Abstractions.Services;
using LightBoard.Shared.Auth;
using Microsoft.AspNetCore.Http;

namespace LightBoard.Application.Services;

public class UserInfoService : IUserInfoService
{
    public Guid UserId { get; }
    public bool IsAuthenticated { get; }

    public UserInfoService(IHttpContextAccessor httpContextAccessor)
    {
        IsAuthenticated = httpContextAccessor.HttpContext is not null &&
                          httpContextAccessor.HttpContext.User is not null &&
                          httpContextAccessor.HttpContext.User.Claims.Any();

        if (!IsAuthenticated)
        {
            return;
        }

        UserId = RetrieveUserId(httpContextAccessor.HttpContext);
    }

    private Guid RetrieveUserId(HttpContext context)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == CustomClaimTypes.UserId);

        if (userIdClaim is null)
        {
            throw new InvalidOperationException("Auth context is not populated with user id");
        }

        if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            throw new Exception("Failed to parse user id");
        }
        
        return userId;
    }
}