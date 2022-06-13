using FluentValidation;
using LightBoard.Application.Models.Boards;

namespace LightBoard.Api.Validators.Boards;

public class CreateBoardRequestValidator : AbstractValidator<CreateBoardRequest>
{
    public CreateBoardRequestValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(32);
    }
}