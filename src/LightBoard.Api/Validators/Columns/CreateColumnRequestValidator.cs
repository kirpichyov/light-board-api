using FluentValidation;
using LightBoard.Application.Models.Columns;

namespace LightBoard.Api.Validators.Columns;

public class CreateColumnRequestValidator : AbstractValidator<CreateColumnRequest>
{
    public CreateColumnRequestValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(32);
    }
}