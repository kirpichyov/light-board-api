using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Auth;
using LightBoard.Application.Models.Users;
using LightBoard.Domain.Entities.Auth;

namespace LightBoard.Application.Mapping;

public class ApplicationMapper : IApplicationMapper
{
    public User ToUser(RegisterRequest request, IHashingProvider hashingProvider)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        return new User(request.Email, request.Name, hashingProvider.GetHash(request.Password));
    }

    public UserInfoResponse ToUserInfoResponse(User user)
    {
        return new UserInfoResponse()
        {
            Email = user.Email,
            Name = user.Name
        };
    }
    
    public IReadOnlyCollection<TDestination> MapCollection<TSource, TDestination>(ICollection<TSource> sources, Func<TSource, TDestination> rule)
    {
        return sources.Select(rule).ToArray();
    }
}