using FluentValidation;
using LightBoard.Application.Models.Users;

namespace LightBoard.Api.Validators
{
    public class UpdateAvatarRequestValidator : AbstractValidator<UpdateAvatarRequest>
    {
        private const int MaxAllowedImageSizeBytes = 10000000;

        private static readonly string[] AllowedImageContentTypes =
        {
            "image/jpeg",
            "image/png"
        };

        public UpdateAvatarRequestValidator()
        {
            RuleFor(request => request.File.Length)
                .GreaterThan(0)
                .WithMessage("File empty");

            RuleFor(request => request.File.ContentType)
                .NotEmpty();

            RuleFor(request => request.File.Length)
                .LessThanOrEqualTo(MaxAllowedImageSizeBytes)
                .WithMessage("Violate max size");

            RuleFor(request => request.File.ContentType)
                .Must(contentType => AllowedImageContentTypes.Contains(contentType))
                .WithMessage("File extension forbiden");

        }
    }
}
