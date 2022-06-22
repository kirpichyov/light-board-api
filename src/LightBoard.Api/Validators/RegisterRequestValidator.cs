using System.Net.Mail;
using System.Text.RegularExpressions;
using FluentValidation;
using LightBoard.Api.Validators.Internal;
using LightBoard.Application.Models.Auth;

namespace LightBoard.Api.Validators;

public class RegisterRequestValidator : PasswordRequestValidator<RegisterRequest>
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
        
        RuleFor(model => model.Surname)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(32);
    }
}