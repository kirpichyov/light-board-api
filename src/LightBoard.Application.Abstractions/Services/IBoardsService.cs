using LightBoard.Application.Models.Boards;
using LightBoard.Application.Models.Cards;
using LightBoard.Application.Models.Cards.Filters;
using LightBoard.Application.Models.Columns;

namespace LightBoard.Application.Abstractions.Services;

public interface IBoardsService
{
    Task<BoardResponse> CreateBoard(CreateBoardRequest request);
    Task<BoardResponse> UpdateBoard(Guid id, UpdateBoardRequest request);
    Task DeleteBoard(Guid id);
    Task<IReadOnlyCollection<BoardResponse>> GetAllBoards();
    Task<BoardResponse> GetBoard(Guid id);
    Task<BoardMemberResponse> InviteMemberToBoard(Guid id, InviteMemberToBoardRequest request);
    Task DeleteBoardMember(Guid boardMemberId);
    Task<IReadOnlyCollection<BoardMemberResponse>> GetAllBoardMembers(Guid id);
    Task<ColumnResponse> CreateColumn(Guid id, CreateColumnRequest request);
    Task<IReadOnlyCollection<ColumnResponse>> GetColumns(Guid id);
    Task<IReadOnlyCollection<CardResponse>> SearchCards(Guid boardId, CardsSearchRequest request);
    Task<IReadOnlyCollection<CardResponse>> GetFilteredCards(Guid boardId, GetCardsFilterRequest getCardsFilterRequest);
}