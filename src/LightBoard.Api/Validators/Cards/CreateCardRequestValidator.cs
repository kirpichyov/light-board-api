using FluentValidation;
using LightBoard.Application.Models.Cards;

namespace LightBoard.Api.Validators.Cards;

public class CreateCardRequestValidator : AbstractValidator<CreateCardRequest>
{
    public CreateCardRequestValidator()
    {
        RuleFor(model => model.Description)
            .NotEmpty()
            .MaximumLength(8192);

        RuleFor(model => model.Title)
            .NotEmpty()
            .MaximumLength(64);
    }
}