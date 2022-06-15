using System.Text.RegularExpressions;
using FluentValidation;
using LightBoard.Application.Models.Auth.Internal;

namespace LightBoard.Api.Validators.Internal;

public class PasswordRequestValidator<T> : AbstractValidator<T>
    where T : class, IContainsPassword
{
    public PasswordRequestValidator()
    {
        RuleFor(model => model.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(32);

        RuleFor(model => model.Password)
            .NotEmpty()
            .Must(password => Regex.IsMatch(password, @"(?=.*[\W_])"))
            .WithMessage("{PropertyName} must have at least 1 special character")
            .Must(password => Regex.IsMatch(password, @"(?=.*\d)"))
            .WithMessage("{PropertyName} must have at least 1 number")
            .Must(password => Regex.IsMatch(password, @"(?=.*[A-Z])"))
            .WithMessage("{PropertyName} must have at least 1 upper case character")
            .When(model => !string.IsNullOrEmpty(model.Password));
    }
}