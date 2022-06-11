using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Results;

namespace LightBoard.Application.Abstractions.Services;

/// <summary>
/// Azure BLOB storage client wrapper.
/// </summary>
public interface IBlobService
{
    /// <summary>
    /// Uploads the blob to storage.
    /// </summary>
    /// <param name="args">Arguments with properties for blob to upload.</param>
    /// <returns>Upload result data.</returns>
    Task<UploadBlobResult> UploadBlob(UploadBlobArgs args);

    /// <summary>
    /// Removes the blob from storage.
    /// </summary>
    /// <param name="container">Container with blob to delete.</param>
    /// <param name="blobName">Name of the blob to delete.</param>
    Task DeleteFile(BlobContainer container, string blobName);
}