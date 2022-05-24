using System.Net.Mail;
using System.Text.RegularExpressions;
using FluentValidation;
using LightBoard.Application.Models.Auth;

namespace LightBoard.Api.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(model => model.Email)
            .NotEmpty()
            .Must(email => MailAddress.TryCreate(email, out _))
            .WithMessage("{PropertyName} has invalid format");

        RuleFor(model => model.Name)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(32);

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