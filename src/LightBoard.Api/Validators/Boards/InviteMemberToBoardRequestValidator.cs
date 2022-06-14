using FluentValidation;
using LightBoard.Application.Models.Boards;

namespace LightBoard.Api.Validators.Boards;

public class InviteMemberToBoardRequestValidator : AbstractValidator<InviteMemberToBoardRequest>
{
    public InviteMemberToBoardRequestValidator()
    {
        RuleFor(model => model.Email)
            .NotEmpty();
    }
}