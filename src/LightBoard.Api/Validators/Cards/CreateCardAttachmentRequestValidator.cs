using FluentValidation;
using LightBoard.Application.Models.Cards;

namespace LightBoard.Api.Validators.Cards
{
    public class CreateCardAttachmentRequestValidator : AbstractValidator<AddCardAttachmentRequest>
    {
        private const int MaxAllowedFileSize = 256000000;

        public CreateCardAttachmentRequestValidator()
        {
            RuleFor(request => request.File.Length)
                .GreaterThan(0)
                .WithMessage("File empty");

            RuleFor(request => request.File.Length)
                .LessThanOrEqualTo(MaxAllowedFileSize)
                .WithMessage("File size can't be greater than 256 MB");
        }
    }
}
