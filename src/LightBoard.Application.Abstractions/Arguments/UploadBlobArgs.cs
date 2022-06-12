using Microsoft.AspNetCore.Http;

namespace LightBoard.Application.Abstractions.Arguments;

public class UploadBlobArgs
{
    /// <summary>
    /// Container where blob should be stored.
    /// </summary>
    public BlobContainer BlobContainer { get; init; }

    /// <summary>
    /// Blob to store represented by <see cref="IFormFile"/>.
    /// </summary>
    public IFormFile FormFile { get; init; }

    /// <summary>
    /// Determines the purpose of the blob.
    /// <remarks>
    ///     This value will be used to form the appropriate metadata.
    /// </remarks>
    /// </summary>
    public BlobPurpose BlobPurpose { get; init; }
}