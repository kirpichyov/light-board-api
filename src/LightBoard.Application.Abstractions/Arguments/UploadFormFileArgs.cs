using Microsoft.AspNetCore.Http;

namespace LightBoard.Application.Abstractions.Arguments;

public class UploadFormFileArgs
{
    public BlobContainer Container { get; init; }
    public IFormFile FormFile { get; init; }
    public BlobPurpose Purpose { get; init; }
}