﻿using LightBoard.Application.Abstractions.Services;
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
    ActionHistoryRecord ToActionHistoryRecord<TItem>(HistoryRecordArgs<TItem> historyRecordArgs)
        where TItem : IPureCloneable;
    HistoryRecordResponse ToHistoryRecordResponse(ActionHistoryRecord actionHistoryRecord);
    CardAttachmentResponse ToCardAttachmentResponse(CardAttachment cardAttachment);
    CardAssigneeResponse ToCardAssigneeResponse(CardAssignee cardAssignee);
    SearchCardsArgs MapToSearchArgs(CardsSearchRequest request);
    PriorityModel ToPriorityModel(Priority priority);
    Priority ToPriority(PriorityModel priorityModel);
    CommentResponse ToCommentResponse(CardComment comment);
    IReadOnlyCollection<TDestination>? MapCollection<TSource, TDestination>(IEnumerable<TSource>? sources, Func<TSource, TDestination> rule);

    IReadOnlyCollection<TDestination> MapCollectionOrEmpty<TSource, TDestination>(IEnumerable<TSource>? sources,
        Func<TSource, TDestination> rule);
}