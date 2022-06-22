using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Auth;
using LightBoard.Application.Models.Boards;
using LightBoard.Application.Models.Cards;
using LightBoard.Application.Models.Columns;
using LightBoard.Application.Models.Users;
using LightBoard.Domain.Entities.Attachments;
using LightBoard.Domain.Entities.Auth;
using LightBoard.Domain.Entities.Boards;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Domain.Entities.Columns;

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
    
    public BoardResponse ToBoardResponse(Board board)
    {
        return new BoardResponse()
        {
            Id = board.Id,
            Name = board.Name,
            BackgroundUrl = board.BackgroundUrl,
            Columns = MapCollectionOrEmpty(board.Columns, ToColumnResponse)
        };    
    }

    public BoardMemberResponse ToBoardMemberResponse(BoardMember boardMember)
    {
        return new BoardMemberResponse()
        {
            Id = boardMember.Id,
            Email = boardMember.User.Email,
            Username = boardMember.User.Name,
            UserAvatar = boardMember.User.AvatarUrl
        };
    }

    public UserProfileResponse ToUserProfileResponse(User user)
    {
        return new UserProfileResponse() 
        {
            Email = user.Email,
            Name = user.Name,
            UserAvatar = user.AvatarUrl
        };
    }

    public ColumnResponse ToColumnResponse(Column column)
    {
        return new ColumnResponse()
        {
            Id = column.Id,
            Name = column.Name,
            Order = column.Order
        };
    }

    public CardResponse ToCardResponse(Card card)
    {
        return new CardResponse()
        {
            Id = card.Id,
            Title = card.Title,
            Description = card.Description,
            Order = card.Order,
            Assignees = MapCollectionOrEmpty(card.CardAssignees, ToAssigneeResponse),
            Attachments = MapCollectionOrEmpty(card.Attachments, ToCardAttachmentResponse),
        };
    }

    public CardAttachmentResponse ToCardAttachmentResponse(CardAttachment cardAttachment)
    {
        return new CardAttachmentResponse()
        {
            Name = cardAttachment.Name,
            UploadedAtUtc = cardAttachment.UploadedAtUtc,
            Url = cardAttachment.Url,
        };
    }

    public CardAssigneeResponse ToCardAssigneeResponse(CardAssignee cardAssignee)
    {
        return new CardAssigneeResponse()
        {
            Id = cardAssignee.Id,
            CardId = cardAssignee.CardId,
            UserId = cardAssignee.UserId
        };
    }

    public IReadOnlyCollection<TDestination>? MapCollection<TSource, TDestination>(IEnumerable<TSource>? sources, Func<TSource, TDestination> rule)
    {
        return sources?.Select(rule).ToArray();
    }

    private IReadOnlyCollection<TDestination> MapCollectionOrEmpty<TSource, TDestination>(IEnumerable<TSource>? sources, Func<TSource, TDestination> rule)
    {
        return MapCollection(sources, rule) ?? Array.Empty<TDestination>();
    }

    private AssigneeResponse ToAssigneeResponse(CardAssignee cardAssignee)
    {
        return new AssigneeResponse()
        {
            Id = cardAssignee.Id,
            Name = cardAssignee.User.Name,
            UserId = cardAssignee.UserId
        };
    }
}