using LightBoard.Api.Swagger.Models;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.CardComments;
using LightBoard.Application.Models.Cards;
using Microsoft.AspNetCore.Mvc;

namespace LightBoard.Api.Controllers;

public class CardsController : ApiControllerBase
{
    private readonly ICardsService _cardsService;
    private readonly ICardCommentsService _cardCommentsService;
    private readonly IUserInfoService _userInfoService;

    public CardsController(
        ICardsService cardsService, 
        ICardCommentsService cardCommentsService, 
        IUserInfoService userInfoService)
    {
        _cardsService = cardsService;
        _cardCommentsService = cardCommentsService;
        _userInfoService = userInfoService;
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

    [HttpDelete("{cardId:guid}")]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCard([FromRoute] Guid cardId)
    {
        await _cardsService.DeleteCard(cardId);

        return NoContent();
    }

    [HttpGet("{cardId:guid}")]
    [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<CardResponse> GetCard([FromRoute] Guid cardId)
    {
        var card = await _cardsService.GetCard(cardId);

        return card;
    }

    [HttpPut("{cardId:guid}/order")]
    [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<CardResponse> UpdateOrder([FromRoute] Guid cardId, [FromBody] UpdateCardOrderRequest request)
    {
        var card = await _cardsService.UpdateOrder(cardId, request);

        return card;
    }

    [HttpPost("{cardId:guid}/assignees")]
    [ProducesResponseType(typeof(CardAssigneeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<CardAssigneeResponse> AddAssigneeToCard([FromRoute] Guid cardId, [FromBody] AddAssigneeToCardRequest request)
    {
        var cardAssignee = await _cardsService.AddAssigneeToCard(cardId, request);

        Response.StatusCode = StatusCodes.Status201Created;

        return cardAssignee;
    }

    [HttpDelete("assignees/{assigneeCardId:guid}")]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAssigneeFromCard([FromRoute] Guid assigneeCardId)
    {
        await _cardsService.DeleteAssigneeFromCard(assigneeCardId);

        return NoContent();
    }


    [HttpPost("{cardId:guid}/attachments")]
    [ProducesResponseType(typeof(CardAttachmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<CardAttachmentResponse> AddAttachment([FromRoute] Guid cardId, [FromForm] AddCardAttachmentRequest request)
    {
        return await _cardsService.AddAttachment(cardId, request);
    }

    [HttpPost("{cardId:guid}/comments")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status400BadRequest)]
    public async Task<CommentResponse> CreateComment([FromRoute] Guid cardId, [FromBody] CreateCommentRequest request)
    {
        return await _cardCommentsService.CreateComment(cardId, _userInfoService.UserId, request.Message);
    }

    [HttpGet("{cardId:guid}/comments")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(EmptyModel), StatusCodes.Status404NotFound)]
    public async Task<IReadOnlyCollection<CommentResponse>> GetComments([FromRoute] Guid cardId)
    {
        return await _cardCommentsService.GetComments(cardId);
    }
}