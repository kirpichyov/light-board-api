using LightBoard.Api.Swagger.Models;
using LightBoard.Application.Abstractions.Services;
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

    [HttpPost("{columnId:guid}")]
    [ProducesResponseType(typeof(ColumnResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<ColumnResponse> UpdateColumn([FromRoute] Guid columnId, [FromBody] UpdateColumnNameRequest request)
    {
        var column = await _columnsService.UpdateColumn(columnId, request);

        return column;
    }

    [HttpDelete("{columnId:guid}")]
    [ProducesResponseType(typeof(ColumnResponse), StatusCodes.Status200OK)]
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
}