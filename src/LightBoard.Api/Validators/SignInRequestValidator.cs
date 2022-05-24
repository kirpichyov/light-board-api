using FluentValidation;
using LightBoard.Application.Models.Auth;

namespace LightBoard.Api.Validators;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(model => model.Email).NotEmpty();
        RuleFor(model => model.Password).NotEmpty();
    }
}