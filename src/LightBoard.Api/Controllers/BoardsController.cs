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
}