using FluentValidation;
using LightBoard.Application.Models.Columns;

namespace LightBoard.Api.Validators.Columns;

public class UpdateColumnNameRequestValidator  : AbstractValidator<UpdateColumnNameRequest>
{
    public UpdateColumnNameRequestValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(32);
    }
}