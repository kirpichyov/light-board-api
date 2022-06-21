using FluentValidation;
using LightBoard.Application.Models.Boards;

namespace LightBoard.Api.Validators.Boards;

public class CreateBoardRequestValidator : AbstractValidator<CreateBoardRequest>
{
    private const int MaxAllowedImageSizeBytes = 10000000;

    private static readonly string[] AllowedImageContentTypes =
    {
        "image/jpeg",
        "image/png"
    };
    public CreateBoardRequestValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(32);

        When(request => request.BoardBackground != null, () =>
        {
            RuleFor(request => request.BoardBackground.Length)
                .GreaterThan(0)
                .WithMessage("File empty");

            RuleFor(request => request.BoardBackground.ContentType)
                .NotEmpty();

            RuleFor(request => request.BoardBackground.Length)
                .LessThanOrEqualTo(MaxAllowedImageSizeBytes)
                .WithMessage("Violate max size");

            RuleFor(request => request.BoardBackground.ContentType)
                .Must(contentType => AllowedImageContentTypes.Contains(contentType))
                .WithMessage("File extension forbiden");
        });
    }
}