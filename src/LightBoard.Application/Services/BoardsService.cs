﻿using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Boards;
using LightBoard.DataAccess.Abstractions;
using LightBoard.Domain.Entities.Boards;
using LightBoard.Shared.Exceptions;

namespace LightBoard.Application.Services;

public class BoardsService : IBoardsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfoService _userInfo;
    private readonly IApplicationMapper _mapper;
    
    public BoardsService(
        IUnitOfWork unitOfWork, 
        IUserInfoService userInfo, 
        IApplicationMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userInfo = userInfo;
        _mapper = mapper;
    }

    public async Task<BoardResponse> CreateBoard(CreateBoardRequest request)
    {
        var board = new Board(request.Name);

        var boardMember = new BoardMember(_userInfo.UserId, board.Id);
        
        _unitOfWork.Boards.Add(board);
        _unitOfWork.BoardMembers.Add(boardMember);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToBoardResponse(board);
    }

    public async Task<BoardResponse> UpdateBoard(Guid id, UpdateBoardRequest request)
    {
        Board board = await _unitOfWork.Boards.GetForUser(id, _userInfo.UserId);

        board.Name = request.Name;
        
        _unitOfWork.Boards.Update(board);
        
        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToBoardResponse(board);
    }

    public async Task DeleteBoard(Guid id)
    {
        Board board = await _unitOfWork.Boards.GetForUser(id, _userInfo.UserId);

        _unitOfWork.Boards.Delete(board);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<BoardResponse>> GetAllBoards()
    {
        var boards = await _unitOfWork.Boards.GetAllByUserId(_userInfo.UserId);

        return _mapper.MapCollection(boards, _mapper.ToBoardResponse);
    }

    public async Task<BoardResponse> GetBoard(Guid id)
    {
        Board board = await _unitOfWork.Boards.GetForUser(id, _userInfo.UserId);

        return _mapper.ToBoardResponse(board);
    }

    public async Task<BoardMemberResponse> InviteMemberToBoard(Guid id, InviteMemberToBoardRequest request)
    {
        var user = await _unitOfWork.Users.Get(request.Email);

        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        var board = await _unitOfWork.Boards.GetForUser(id, _userInfo.UserId);

        if (board.BoardMembers.Any(member => member.UserId == user.Id))
        {
            throw new ValidationFailedException("User already has access to board");
        }

        BoardMember member = new BoardMember(user.Id, board.Id);
        
        _unitOfWork.BoardMembers.Add(member);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToBoardMemberResponse(member);
    }

    public async Task DeleteBoardMember(Guid boardMemberId)
    {
        var boardMember = await _unitOfWork.BoardMembers.GetById(boardMemberId);

        if (boardMember.UserId == _userInfo.UserId)
        {
            throw new AccessDeniedException();
        }
        
        if (!await _unitOfWork.Boards.HasAccessToBoard(boardMember.BoardId, _userInfo.UserId))
        {
            throw new AccessDeniedException();
        }
        
        _unitOfWork.BoardMembers.Delete(boardMember);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<BoardMemberResponse>> GetAllBoardMembers(Guid id)
    {
        var board = await _unitOfWork.Boards.GetForUser(id, _userInfo.UserId);
        
        return _mapper.MapCollection(board.BoardMembers, _mapper.ToBoardMemberResponse);
    }
}