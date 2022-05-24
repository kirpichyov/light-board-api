using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Services;
using LightBoard.Shared.Api;

namespace LightBoard.Api.Middleware.SessionKey;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSessionKeyAuthorization(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddAuthentication(options => options.DefaultScheme = ApiSchemas.SessionKeyScheme)
            .AddScheme<SessionAuthSchemeOptions, SessionAuthHandler>(ApiSchemas.SessionKeyScheme, _ => { });
        
        services.AddScoped<IUserInfoService, UserInfoService>();

        return services;
    }
}