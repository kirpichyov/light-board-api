using FluentValidation;
using LightBoard.Application.Models.Boards;

namespace LightBoard.Api.Validators.Boards;

public class UpdateBoardRequestValidator : AbstractValidator<UpdateBoardRequest>
{
    private const int MaxAllowedImageSizeBytes = 10000000;

    private static readonly string[] AllowedImageContentTypes =
        {
            "image/jpeg",
            "image/png"
        };

    public UpdateBoardRequestValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(32);

        When(request => request.Background != null, () =>
        {
            RuleFor(request => request.Background.Length)
                .GreaterThan(0)
                .WithMessage("File empty");

            RuleFor(request => request.Background.ContentType)
                .NotEmpty();

            RuleFor(request => request.Background.Length)
                .LessThanOrEqualTo(MaxAllowedImageSizeBytes)
                .WithMessage("Violate max size");

            RuleFor(request => request.Background.ContentType)
                .Must(contentType => AllowedImageContentTypes.Contains(contentType))
                .WithMessage("File extension forbiden");
        });
    }
}