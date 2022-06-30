using FluentValidation;
using LightBoard.Application.Models.Columns;

namespace LightBoard.Api.Validators.Columns;

public class UpdateColumnOrderRequestValidator : AbstractValidator<UpdateColumnOrderRequest>
{
    public UpdateColumnOrderRequestValidator()
    {
        RuleFor(model => model.Order)
            .NotEmpty()
            .GreaterThanOrEqualTo(1);
    }
}