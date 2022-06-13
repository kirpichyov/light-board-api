using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Boards;
using LightBoard.DataAccess.Abstractions;
using LightBoard.Domain.Entities.Boards;

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
}