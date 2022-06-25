using FluentValidation;
using LightBoard.Application.Models.CardComments;

namespace LightBoard.Api.Validators.Cards
{
    public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
    {
        public CreateCommentRequestValidator()
        {
            RuleFor(model => model.Message)
                .NotEmpty()
                .MaximumLength(2048);
        }
    }
}
