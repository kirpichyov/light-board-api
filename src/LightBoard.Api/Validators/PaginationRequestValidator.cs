using FluentValidation;
using LightBoard.Application.Models.Paginations;

namespace LightBoard.Api.Validators;

public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
{
    public PaginationRequestValidator()
    {
        RuleFor(model => model.Take)
            .NotEmpty()
            .LessThanOrEqualTo(1000)
            .GreaterThanOrEqualTo(1);
    }
}