using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Auth;
using LightBoard.Application.Models.Boards;
using LightBoard.Application.Models.Users;
using LightBoard.Domain.Entities.Auth;
using LightBoard.Domain.Entities.Boards;

namespace LightBoard.Application.Abstractions.Mapping;

public interface IApplicationMapper
{
    User ToUser(RegisterRequest request, IHashingProvider hashingProvider);
    UserInfoResponse ToUserInfoResponse(User user);
    BoardResponse ToBoardResponse(Board board);
    BoardMemberResponse ToBoardMemberResponse(BoardMember boardMember);
    IReadOnlyCollection<TDestination> MapCollection<TSource, TDestination>(IEnumerable<TSource> sources, Func<TSource, TDestination> rule);
    UserProfileResponse ToUserProfileResponse(User user);
}