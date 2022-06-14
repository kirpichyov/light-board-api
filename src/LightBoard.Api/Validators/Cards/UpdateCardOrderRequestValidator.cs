using FluentValidation;
using LightBoard.Application.Models.Cards;

namespace LightBoard.Api.Validators.Cards;

public class UpdateCardOrderRequestValidator : AbstractValidator<UpdateCardOrderRequest>
{
    public UpdateCardOrderRequestValidator()
    {
        RuleFor(model => model.Order)
            .NotEmpty()
            .GreaterThanOrEqualTo(1);
    }
}