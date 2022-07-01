using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Auth;
using LightBoard.Application.Models.Boards;
using LightBoard.Application.Models.CardComments;
using LightBoard.Application.Models.Cards;
using LightBoard.Application.Models.Columns;
using LightBoard.Application.Models.Enums;
using LightBoard.Application.Models.Records;
using LightBoard.Application.Models.Users;
using LightBoard.DataAccess.Abstractions.Arguments;
using LightBoard.Domain.Entities.Attachments;
using LightBoard.Domain.Entities.Auth;
using LightBoard.Domain.Entities.Boards;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Domain.Entities.Columns;
using LightBoard.Domain.Entities.Record;
using LightBoard.Domain.Enums;
using LightBoard.Shared.Contracts;
using Newtonsoft.Json;

namespace LightBoard.Application.Mapping;

public class ApplicationMapper : IApplicationMapper
{
    public User ToUser(RegisterRequest request, IHashingProvider hashingProvider)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        return new User(request.Email, request.Name, request.Surname, hashingProvider.GetHash(request.Password));
    }

    public UserInfoResponse ToUserInfoResponse(User user)
    {
        return new UserInfoResponse()
        {
            Email = user.Email,
            FullName = user.FullName
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
            Surname = user.Surname,
            FullName = user.FullName,
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
            DeadlineAtUtc = card.DeadlineAtUtc,
            Priority = ToPriorityModel(card.Priority),
            Assignees = MapCollectionOrEmpty(card.CardAssignees, ToAssigneeResponse),
            Attachments = MapCollectionOrEmpty(card.Attachments, ToCardAttachmentResponse),
        };
    }

    public ActionHistoryRecord ToActionHistoryRecord<TItem>(HistoryRecordArgs<TItem> historyRecordArgs)
        where TItem : IPureCloneable
    {
        return new ActionHistoryRecord(
            historyRecordArgs.UserId,
            historyRecordArgs.ResourceId,
            historyRecordArgs.ResourceType,
            historyRecordArgs.ActionType,
            historyRecordArgs.CreatedTime,
            historyRecordArgs.OldValue,
            historyRecordArgs.NewValue,
            historyRecordArgs.BoardId
            );
    }

    public HistoryRecordResponse ToHistoryRecordResponse(ActionHistoryRecord actionHistoryRecord)
    {
        return new HistoryRecordResponse
        {
            UserId = actionHistoryRecord.UserId,
            ActionType = actionHistoryRecord.ActionType,
            CreatedTime = actionHistoryRecord.CreatedTime,
            ResourceId = actionHistoryRecord.ResourceId,
            ResourceType = actionHistoryRecord.ResourceType,
            NewValue = actionHistoryRecord.NewValue is null
                ? null
                : JsonConvert.DeserializeObject(actionHistoryRecord.NewValue),
            OldValue = actionHistoryRecord.OldValue is null
                ? null
                : JsonConvert.DeserializeObject(actionHistoryRecord.OldValue)
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

    public IReadOnlyCollection<TDestination> MapCollectionOrEmpty<TSource, TDestination>(IEnumerable<TSource>? sources, Func<TSource, TDestination> rule)
    {
        return MapCollection(sources, rule) ?? Array.Empty<TDestination>();
    }

    private AssigneeResponse ToAssigneeResponse(CardAssignee cardAssignee)
    {
        return new AssigneeResponse()
        {
            Id = cardAssignee.Id,
            FullName = cardAssignee.User.FullName,
            UserId = cardAssignee.UserId
        };
    }

    public PriorityModel ToPriorityModel(Priority priority)
    {
        return priority switch
        {
            Priority.None => PriorityModel.None,
            Priority.Low => PriorityModel.Low,
            Priority.Lowest => PriorityModel.Lowest,
            Priority.High => PriorityModel.High,
            Priority.Highest => PriorityModel.Highest,
            Priority.Normal => PriorityModel.Normal
        };
    }

    public Priority ToPriority(PriorityModel priorityModel)
    {
        return priorityModel switch
        {
            PriorityModel.None => Priority.None,
            PriorityModel.Low => Priority.Low,
            PriorityModel.Lowest => Priority.Lowest,
            PriorityModel.High => Priority.High,
            PriorityModel.Highest => Priority.Highest ,
            PriorityModel.Normal => Priority.Normal
        };
    }

    public CommentResponse ToCommentResponse(CardComment comment)
    {
        return new CommentResponse()
        {
            Id = comment.Id,
            CardId = comment.CardId,
            UserId = comment.UserId,
            Message = comment.Message,
            CreatedAtUtc = comment.CreatedAtUtc
        };
    }

    public SearchCardsArgs MapToSearchArgs(SearchCardsRequest request)
    {
        return new SearchCardsArgs()
        {
            Text = request.Text,
            SearchInDescription = request.SearchInDescription,
            SearchInTitle = request.SearchInTitle
        };
    }
}