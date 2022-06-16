using LightBoard.Api.Swagger.Models;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Cards;
using Microsoft.AspNetCore.Mvc;

namespace LightBoard.Api.Controllers;

public class CardsController : ApiControllerBase
{
    private readonly ICardsService _cardsService;

    public CardsController(ICardsService cardsService)
    {
        _cardsService = cardsService;
    }

    [HttpPut("{cardId:guid}")]
    [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<CardResponse> UpdateCard([FromRoute] Guid cardId, [FromBody] UpdateCardRequest request)
    {
        var card = await _cardsService.UpdateCard(cardId, request);

        return card;
    }

    [HttpDelete("cards/{cardId:guid}")]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCard([FromRoute] Guid cardId)
    {
        await _cardsService.DeleteCard(cardId);

        return NoContent();
    }

    [HttpGet("cards/{cardId:guid}")]
    [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<CardResponse> GetCard([FromRoute] Guid cardId)
    {
        var card = await _cardsService.GetCard(cardId);

        return card;
    }

    [HttpPut("cards/{cardId:guid}/order")]
    [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<CardResponse> UpdateOrder([FromRoute] Guid cardId, [FromBody] UpdateCardOrderRequest request)
    {
        var card = await _cardsService.UpdateOrder(cardId, request);

        return card;
    }
}