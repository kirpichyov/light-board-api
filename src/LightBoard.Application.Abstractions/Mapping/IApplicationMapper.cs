using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Auth;
using LightBoard.Application.Models.Users;
using LightBoard.Domain.Entities.Auth;

namespace LightBoard.Application.Abstractions.Mapping;

public interface IApplicationMapper
{
    User ToUser(RegisterRequest request, IHashingProvider hashingProvider);
    UserInfoResponse ToUserInfoResponse(User user);
    IReadOnlyCollection<TDestination> MapCollection<TSource, TDestination>(ICollection<TSource> sources, Func<TSource, TDestination> rule);
}