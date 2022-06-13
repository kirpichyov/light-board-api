using LightBoard.Api.Swagger.Models;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Boards;
using Microsoft.AspNetCore.Mvc;

namespace LightBoard.Api.Controllers;

public class BoardsController : ApiControllerBase
{
    private readonly IBoardsService _boardsService;

    public BoardsController(IBoardsService boardsService)
    {
        _boardsService = boardsService;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(BoardResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<BoardResponse> CreateBoard([FromBody] CreateBoardRequest request)
    {
        var board = await _boardsService.CreateBoard(request);

        Response.StatusCode = StatusCodes.Status201Created;
        
        return board;
    }
    
    [HttpPut("{boardId:guid}")]
    [ProducesResponseType(typeof(BoardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<BoardResponse> UpdateBoard([FromRoute] Guid boardId, [FromBody] UpdateBoardRequest request)
    {
        var board = await _boardsService.UpdateBoard(boardId, request);
        
        return board;
    }
    
    [HttpDelete("{boardId:guid}")]
    [ProducesResponseType(typeof(BoardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBoard([FromRoute] Guid boardId)
    {
        await _boardsService.DeleteBoard(boardId);
        
        return NoContent();
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(BoardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<IReadOnlyCollection<BoardResponse>> GetAllBoards()
    {
        var boards = await _boardsService.GetAllBoards();
        
        return boards;
    }
    
    [HttpGet("{boardId:guid}")]
    [ProducesResponseType(typeof(BoardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<BoardResponse> GetBoard([FromRoute] Guid boardId)
    {
        var board = await _boardsService.GetBoard(boardId);
        
        return board;
    }

    [HttpPost("{boardId:guid}/invite-member")]
    [ProducesResponseType(typeof(BoardMemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<BoardMemberResponse> InviteMemberToBoard([FromRoute] Guid boardId, [FromBody] InviteMemberToBoardRequest request)
    {
        var boardMember = await _boardsService.InviteMemberToBoard(boardId, request);

        return boardMember;
    }
    
    [HttpDelete("members/{boardMemberId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBoardMember([FromRoute] Guid boardMemberId)
    {
        await _boardsService.DeleteBoardMember(boardMemberId);
        
        return NoContent();
    }

    [HttpGet("{boardId:guid}/members")]
    [ProducesResponseType(typeof(BoardMemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<IReadOnlyCollection<BoardMemberResponse>> GetAllBoardMembers([FromRoute] Guid boardId)
    {
        var boardMembers = await _boardsService.GetAllBoardMembers(boardId);

        return boardMembers;
    }
}