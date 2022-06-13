using FluentValidation;
using LightBoard.Application.Models.Boards;

namespace LightBoard.Api.Validators.Boards;

public class UpdateBoardRequestValidator : AbstractValidator<UpdateBoardRequest>
{
    public UpdateBoardRequestValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(32);
    }
}