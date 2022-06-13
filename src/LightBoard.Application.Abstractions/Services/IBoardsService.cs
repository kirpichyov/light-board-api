﻿using LightBoard.Application.Models.Boards;

namespace LightBoard.Application.Abstractions.Services;

public interface IBoardsService
{
    Task<BoardResponse> CreateBoard(CreateBoardRequest request);
    Task<BoardResponse> UpdateBoard(Guid id, UpdateBoardRequest request);
    Task DeleteBoard(Guid id);
    Task<IReadOnlyCollection<BoardResponse>> GetAllBoards();
    Task<BoardResponse> GetBoard(Guid id);
    Task<BoardMemberResponse> InviteMemberToBoard(Guid id, InviteMemberToBoardRequest request);
    Task DeleteBoardMember(Guid id, Guid userId);
    Task<IReadOnlyCollection<BoardMemberResponse>> GetAllBoardMembers(Guid id);
}