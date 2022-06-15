using FluentValidation;
using LightBoard.Api.Validators.Internal;
using LightBoard.Application.Models.Auth;

namespace LightBoard.Api.Validators;

public class UpdatePasswordValidator : PasswordRequestValidator<UpdatePasswordRequest>
{
    public UpdatePasswordValidator()
    {
        RuleFor(model => model.OldPassword)
            .NotEmpty();
    }
}