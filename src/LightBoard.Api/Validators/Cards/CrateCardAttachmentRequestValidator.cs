using FluentValidation;
using LightBoard.Application.Models.Cards;

namespace LightBoard.Api.Validators.Cards
{
    public class CrateCardAttachmentRequestValidator : AbstractValidator<CardAttachmentRequest>
    {
        private const int MaxAllowedFileSize = 256000000;

        public CrateCardAttachmentRequestValidator()
        {
            RuleFor(request => request.File.Length)
                .GreaterThan(0)
                .WithMessage("File empty");

            RuleFor(request => request.File.Length)
                .LessThanOrEqualTo(MaxAllowedFileSize)
                .WithMessage("Violate max size");
        }
    }
}
