﻿using LightBoard.Api.Swagger.Models;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Boards;
using LightBoard.Application.Models.Cards;
using LightBoard.Application.Models.Cards.Filters;
using LightBoard.Application.Models.Columns;
using LightBoard.Application.Models.Paginations;
using LightBoard.Application.Models.Records;
using Microsoft.AspNetCore.Mvc;

namespace LightBoard.Api.Controllers;

public class BoardsController : ApiControllerBase
{
    private readonly IBoardsService _boardsService;
    private readonly IHistoryRecordService _historyRecordService;

    public BoardsController(IBoardsService boardsService, IHistoryRecordService historyRecordService)
    {
        _boardsService = boardsService;
        _historyRecordService = historyRecordService;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(BoardResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<BoardResponse> CreateBoard([FromForm] CreateBoardRequest request)
    {
        var board = await _boardsService.CreateBoard(request);

        Response.StatusCode = StatusCodes.Status201Created;
        
        return board;
    }
    
    [HttpPut("{boardId:guid}")]
    [ProducesResponseType(typeof(BoardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<BoardResponse> UpdateBoard([FromRoute] Guid boardId, [FromForm] UpdateBoardRequest request)
    {
        var board = await _boardsService.UpdateBoard(boardId, request);
        
        return board;
    }
    
    [HttpDelete("{boardId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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
    
    [HttpGet("{boardId:guid}/history")]
    [ProducesResponseType(typeof(IReadOnlyCollection<HistoryRecordResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<IReadOnlyCollection<HistoryRecordResponse>> GetBoardActionHistoryById(
        [FromRoute] Guid boardId, 
        [FromQuery] PaginationRequest paginationRequest)
    {
        return await _historyRecordService.GetAllHistoryRecord(boardId, paginationRequest);
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

    [HttpPost("{boardId:guid}/columns")]
    [ProducesResponseType(typeof(ColumnResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<ColumnResponse> CreateColumn([FromRoute] Guid boardId, [FromBody] CreateColumnRequest request)
    {
        var column = await _boardsService.CreateColumn(boardId, request);
        
        Response.StatusCode = StatusCodes.Status201Created;

        return column;
    }

    [HttpGet("{boardId:guid}/columns")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ColumnResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<IReadOnlyCollection<ColumnResponse>> GetColumns([FromRoute] Guid boardId)
    {
        var columns = await _boardsService.GetColumns(boardId);

        return columns;
    }

    [HttpGet("{boardId:guid}/cards/search")]
    [ProducesResponseType(typeof(IReadOnlyCollection<CardResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<IReadOnlyCollection<CardResponse>> SearchCards([FromRoute] Guid boardId, [FromQuery] CardsSearchRequest request)
    {
        return await _boardsService.SearchCards(boardId, request);
    }

    [HttpGet("{boardId:guid}/cards")]
    public async Task<IReadOnlyCollection<CardResponse>> GetCardsFilter([FromRoute] Guid boardId, [FromQuery] GetCardsFilterRequest getCardsFilterRequest)
    {
        return await _boardsService.GetFilteredCards(boardId, getCardsFilterRequest);
    }
}