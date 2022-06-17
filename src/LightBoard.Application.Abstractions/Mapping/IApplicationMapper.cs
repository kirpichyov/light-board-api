using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Auth;
using LightBoard.Application.Models.Boards;
using LightBoard.Application.Models.Cards;
using LightBoard.Application.Models.Columns;
using LightBoard.Application.Models.Users;
using LightBoard.Domain.Entities.Auth;
using LightBoard.Domain.Entities.Boards;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Domain.Entities.Columns;

namespace LightBoard.Application.Abstractions.Mapping;

public interface IApplicationMapper
{
    User ToUser(RegisterRequest request, IHashingProvider hashingProvider);
    UserInfoResponse ToUserInfoResponse(User user);
    BoardResponse ToBoardResponse(Board board);
    BoardMemberResponse ToBoardMemberResponse(BoardMember boardMember);
    UserProfileResponse ToUserProfileResponse(User user);
    ColumnResponse ToColumnResponse(Column column);
    CardResponse ToCardResponse(Card card);
    CardAssigneeResponse ToCardAssigneeResponse(CardAssignee cardAssignee);
    IReadOnlyCollection<TDestination> MapCollection<TSource, TDestination>(IEnumerable<TSource> sources, Func<TSource, TDestination> rule);
}