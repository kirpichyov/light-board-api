using FluentValidation;
using LightBoard.Api.Validators.Internal;
using LightBoard.Application.Models.Auth;

namespace LightBoard.Api.Validators;

public class ResetPasswordValidator : PasswordRequestValidator<ResetPasswordRequest>
{
    public ResetPasswordValidator()
    {
        RuleFor(model => model.ResetCode)
            .NotEmpty();
    }
}