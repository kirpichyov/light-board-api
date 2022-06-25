using FluentValidation;
using LightBoard.Application.Models.CardComments;

namespace LightBoard.Api.Validators.Cards
{
    public class UpdateCommentRequestValidator : AbstractValidator<UpdateCommentRequest>
    {
        public UpdateCommentRequestValidator()
        {
            RuleFor(model => model.Message)
                .NotEmpty()
                .MaximumLength(2048);
        }
    }
}
