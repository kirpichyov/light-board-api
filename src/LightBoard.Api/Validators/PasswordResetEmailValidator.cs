using System.Net.Mail;
using System.Text.RegularExpressions;
using FluentValidation;
using LightBoard.Application.Models.Auth;

namespace LightBoard.Api.Validators;

public class PasswordResetEmailValidator : AbstractValidator<ResetPasswordEmailRequest>
{
    public PasswordResetEmailValidator()
    {
        RuleFor(model => model.Email)
            .NotEmpty()
            .Must(email => MailAddress.TryCreate(email, out _))
            .WithMessage("{PropertyName} has invalid format");
    }
}