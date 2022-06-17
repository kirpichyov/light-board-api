using FluentValidation;
using LightBoard.Application.Models.Cards;

namespace LightBoard.Api.Validators.Cards;

public class AddAssigneeToCardRequestValidator : AbstractValidator<AddAssigneeToCardRequest>
{
    public AddAssigneeToCardRequestValidator()
    {
        RuleFor(model => model.UserId)
            .NotEmpty();
    }
}