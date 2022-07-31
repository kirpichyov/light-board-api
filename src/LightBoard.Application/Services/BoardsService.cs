using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Boards;
using LightBoard.Application.Models.Cards;
using LightBoard.Application.Models.Cards.Filters;
using LightBoard.Application.Models.Columns;
using LightBoard.Application.Models.Records;
using LightBoard.DataAccess.Abstractions;
using LightBoard.Domain.Entities.Boards;
using LightBoard.Domain.Entities.Columns;
using LightBoard.Domain.Enums;
using LightBoard.Shared.Exceptions;
using LightBoard.Shared.Models.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LightBoard.Application.Services;

public class BoardsService : IBoardsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfoService _userInfo;
    private readonly IApplicationMapper _mapper;
    private readonly IBlobService _blobService;
    private readonly IHistoryRecordService _historyRecordService;
    public BoardsService(
        IUnitOfWork unitOfWork,
        IUserInfoService userInfo,
        IApplicationMapper mapper,
        IBlobService blobService, 
        IHistoryRecordService historyRecordService)
    {
        _unitOfWork = unitOfWork;
        _userInfo = userInfo;
        _mapper = mapper;
        _blobService = blobService;
        _historyRecordService = historyRecordService;
    }

    public async Task<BoardResponse> CreateBoard(CreateBoardRequest request)
    {
        var board = new Board(request.Name);

        var boardMember = new BoardMember(_userInfo.UserId, board.Id);

        if (request.Background != null)
        {
            await UploadBoardBackground(request.Background, board);
        }
        
        var historyRecordsArgs = new HistoryRecordArgs<Board>
        {
            ActionType = ActionType.Create,
            CreatedTime = DateTime.UtcNow,
            ResourceId = board.Id,
            ResourceType = ResourceType.Board,
            UserId = _userInfo.UserId,
            BoardId = board.Id
        };
        
        historyRecordsArgs.SetNewValue(board);

        _unitOfWork.Boards.Add(board);
        _unitOfWork.BoardMembers.Add(boardMember);
        
        _historyRecordService.AddHistoryRecord(historyRecordsArgs);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToBoardResponse(board);
    }

    public async Task<BoardResponse> UpdateBoard(Guid id, UpdateBoardRequest request)
    {
        Board board = await _unitOfWork.Boards.GetForUser(id, _userInfo.UserId);
        
        var historyRecordsArgs = new HistoryRecordArgs<Board>
        {
            ActionType = ActionType.Update,
            CreatedTime = DateTime.UtcNow,
            ResourceId = board.Id,
            ResourceType = ResourceType.Board,
            UserId = _userInfo.UserId,
            BoardId = board.Id
        };
        
        historyRecordsArgs.SetOldValue(board);

        board.Name = request.Name;

        if (request.Background != null)
        {
            await UploadBoardBackground(request.Background, board);
        }

        historyRecordsArgs.SetNewValue(board);

        _unitOfWork.Boards.Update(board);
        
        _historyRecordService.AddHistoryRecord(historyRecordsArgs);

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
        
        var historyRecordsArgs = new HistoryRecordArgs<Column>
        {
            ActionType = ActionType.Create,
            CreatedTime = DateTime.UtcNow,
            ResourceId = column.Id,
            ResourceType = ResourceType.Column,
            UserId = _userInfo.UserId,
            BoardId = column.BoardId
        };
        
        historyRecordsArgs.SetNewValue(column);

        _unitOfWork.Columns.Add(column);
        
        _historyRecordService.AddHistoryRecord(historyRecordsArgs);

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

    public async Task<IReadOnlyCollection<CardResponse>> SearchCards(Guid boardId, CardsSearchRequest request)
    {
        var isHasAccess = await _unitOfWork.Boards.HasAccessToBoard(boardId, _userInfo.UserId);

        if (!isHasAccess)
        {
            throw new NotFoundException("Board is not found");
        }

        var searchArgs = _mapper.MapToSearchArgs(request);
        var cards = await _unitOfWork.Boards.SearchForUser(boardId, searchArgs);

        return _mapper.MapCollection(cards, _mapper.ToCardResponse);
    }

    public async Task<IReadOnlyCollection<CardResponse>> GetFilteredCards(Guid boardId, GetCardsFilterRequest getCardsFilterRequest)
    {
        var cards = await _unitOfWork.Cards.GetFilteredCards(_userInfo.UserId, boardId, getCardsFilterRequest.Assignees,
            getCardsFilterRequest.Direction, getCardsFilterRequest.SortBy);
        
        return _mapper.MapCollection(cards, _mapper.ToCardResponse);
    }
}