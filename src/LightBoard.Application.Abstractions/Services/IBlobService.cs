using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Results;

namespace LightBoard.Application.Abstractions.Services;

public interface IBlobService
{
    Task<UploadBlobResult> UploadFormFile(UploadFormFileArgs arguments);
    Task<UploadBlobResult> UploadStreamContent(UploadFileStreamArgs arguments);
    Task<string> GetBlobStringContentAsync(BlobContainer blobContainer, string fileName);
    Task DeleteFile(BlobContainer container, string blobName);
}