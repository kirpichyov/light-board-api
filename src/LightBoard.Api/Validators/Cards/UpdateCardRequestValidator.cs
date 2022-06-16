using FluentValidation;
using LightBoard.Application.Models.Cards;

namespace LightBoard.Api.Validators.Cards;

public class UpdateCardRequestValidator : AbstractValidator<UpdateCardRequest> 
{
    public UpdateCardRequestValidator()
    {
        RuleFor(model => model.Description)
            .NotEmpty()
            .MaximumLength(8192);

        RuleFor(model => model.Title)
            .NotEmpty()
            .MaximumLength(64);
    }
}