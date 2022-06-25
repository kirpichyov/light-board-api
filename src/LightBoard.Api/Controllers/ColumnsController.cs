using LightBoard.Api.Swagger.Models;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Cards;
using LightBoard.Application.Models.Columns;
using Microsoft.AspNetCore.Mvc;

namespace LightBoard.Api.Controllers;

public class ColumnsController : ApiControllerBase
{
    private readonly IColumnsService _columnsService;

    public ColumnsController(IColumnsService columnsService)
    {
        _columnsService = columnsService;
    }

    [HttpPut("{columnId:guid}")]
    [ProducesResponseType(typeof(ColumnResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<ColumnResponse> UpdateColumn([FromRoute] Guid columnId, [FromBody] UpdateColumnNameRequest request)
    {
        var column = await _columnsService.UpdateColumn(columnId, request);

        return column;
    }

    [HttpDelete("{columnId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteColumn([FromRoute] Guid columnId)
    {
        await _columnsService.DeleteColumn(columnId);

        return NoContent();
    }

    [HttpGet("{columnId:guid}")]
    [ProducesResponseType(typeof(ColumnResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<ColumnResponse> GetColumn([FromRoute] Guid columnId)
    {
        var column = await _columnsService.GetColumn(columnId);

        return column;
    }

    [HttpPut("{columnId:guid}/order")]
    [ProducesResponseType(typeof(ColumnResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<ColumnResponse> UpdateOrderColumn([FromRoute] Guid columnId, [FromBody] UpdateColumnOrderRequest request)
    {
        var column = await _columnsService.UpdateOrder(columnId, request);

        return column;
    }

    [HttpPost("{columnId:guid}/cards")]
    [ProducesResponseType(typeof(ColumnResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<CardResponse> CreateCard([FromRoute] Guid columnId, [FromBody] CreateCardRequest request)
    {
        var card = await _columnsService.CreateCard(columnId, request);

        Response.StatusCode = StatusCodes.Status201Created;

        return card;
    }

    [HttpGet("{columnId:guid}/cards")]
    [ProducesResponseType(typeof(ColumnResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<IReadOnlyCollection<CardResponse>> GetColumnCards([FromRoute] Guid columnId)
    {
        var cards = await _columnsService.GetColumnCards(columnId);

        return cards;
    }
}