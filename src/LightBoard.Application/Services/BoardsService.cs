using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Results;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Boards;
using LightBoard.Application.Models.Columns;
using LightBoard.DataAccess.Abstractions;
using LightBoard.Domain.Entities.Boards;
using LightBoard.Domain.Entities.Columns;
using LightBoard.Shared.Exceptions;
using Microsoft.AspNetCore.Http;

namespace LightBoard.Application.Services;

public class BoardsService : IBoardsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfoService _userInfo;
    private readonly IApplicationMapper _mapper;
    private readonly IBlobService _blobService;
    public BoardsService(
        IUnitOfWork unitOfWork,
        IUserInfoService userInfo,
        IApplicationMapper mapper,
        IBlobService blobService)
    {
        _unitOfWork = unitOfWork;
        _userInfo = userInfo;
        _mapper = mapper;
        _blobService = blobService;
    }

    public async Task<BoardResponse> CreateBoard(CreateBoardRequest request)
    {
        var board = new Board(request.Name);

        var boardMember = new BoardMember(_userInfo.UserId, board.Id);

        if (request.Background != null)
        {
            await UploadBoardBackground(request.Background, board);
        }

        _unitOfWork.Boards.Add(board);
        _unitOfWork.BoardMembers.Add(boardMember);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToBoardResponse(board);
    }

    public async Task<BoardResponse> UpdateBoard(Guid id, UpdateBoardRequest request)
    {
        Board board = await _unitOfWork.Boards.GetForUser(id, _userInfo.UserId);

        board.Name = request.Name;

        if (request.Background != null)
        {
            await UploadBoardBackground(request.Background, board);
        }

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

    public async Task<ColumnResponse> CreateColumn(Guid id, CreateColumnRequest request)
    {
        var board = await _unitOfWork.Boards.GetForUser(id, _userInfo.UserId);

        var column = new Column(request.Name, id, board.Columns.Count + 1);

        _unitOfWork.Columns.Add(column);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToColumnResponse(column);
    }

    public async Task<IReadOnlyCollection<ColumnResponse>> GetColumns(Guid id)
    {
        var board = await _unitOfWork.Boards.GetForUser(id, _userInfo.UserId);

        return _mapper.MapCollection(board.Columns, _mapper.ToColumnResponse);
    }

    private async Task<Board> UploadBoardBackground(IFormFile boardBackground, Board board)
    {
        var args = new UploadFormFileArgs()
        {
            Container = BlobContainer.BoardBackgrounds,
            Purpose = BlobPurpose.Inline,
            FormFile = boardBackground
        };
       
        if (board.BackgroundBlobName != null)
        {
            await _blobService.DeleteFile(BlobContainer.BoardBackgrounds, board.BackgroundBlobName);
        }
        var result = await _blobService.UploadFormFile(args);
        board.BackgroundUrl = result.Uri;
        board.BackgroundBlobName = result.BlobName;
        return board;
    }
}