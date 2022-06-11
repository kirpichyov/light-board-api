using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using GuardNet;
using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Options;
using LightBoard.Application.Abstractions.Results;
using LightBoard.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Unidecode.NET;

namespace LightBoard.Application.Services;

public class BlobService : IBlobService
{
    private static readonly DateTimeOffset DefaultSasExpiresOn = new(new DateTime(9999, 1, 1));
    private readonly BlobOptions _blobOptions;
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(IOptions<BlobOptions> blobOptions)
    {
        _blobOptions = blobOptions.Value;
        _blobServiceClient = new BlobServiceClient(_blobOptions.ConnectionString);
    }
    
    public async Task<UploadBlobResult> UploadBlob(UploadBlobArgs args)
    {
        VerifyArgs(args);

        string containerName = MapToContainerName(args.BlobContainer);
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        
        var blobName = Guid.NewGuid().ToString();
        var blob = containerClient.GetBlobClient(blobName);

        var blobHttpHeader = new BlobHttpHeaders
        {
            ContentType = args.FormFile.ContentType,
            ContentDisposition = GetNormalizedContentDisposition(args.FormFile, args.BlobPurpose),
        };

        await blob.UploadAsync(args.FormFile.OpenReadStream(), blobHttpHeader);
        var uri = blob.GenerateSasUri(BlobSasPermissions.Read, DefaultSasExpiresOn).ToString();

        return new UploadBlobResult(uri, blobName);
    }
    
    public async Task DeleteFile(BlobContainer container, string blobName)
    {
        var containerName = MapToContainerName(container);
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.GetBlobClient(blobName).DeleteAsync();
    }

    private string MapToContainerName(BlobContainer container)
    {
        return container switch
        {
            BlobContainer.UserAvatars => _blobOptions.ContainerNames.UserAvatars,
            BlobContainer.BoardBackgrounds => _blobOptions.ContainerNames.BoardBackgrounds,
            BlobContainer.CardAttachments => _blobOptions.ContainerNames.CardAttachments,
            _ => throw new ArgumentException($"Value '{container}' is unexpected.", nameof(container))
        };
    }

    private string GetNormalizedContentDisposition(IFormFile file, BlobPurpose blobPurpose)
    {
        string type = blobPurpose switch
        {
            BlobPurpose.Inline => "inline",
            BlobPurpose.Attachment => "attachment",
            _ => throw new ArgumentException($"Value '{blobPurpose}' is unexpected.", nameof(blobPurpose))
        };
        
        return $"{type}; filename=\"{file.FileName.Unidecode()}\";";
    }

    private void VerifyArgs(UploadBlobArgs args)
    {
        Guard.NotNull(args.FormFile, nameof(args.FormFile));
        Guard.NotLessThan(args.FormFile.Length, 1, nameof(args.FormFile.Length), "File is empty.");
    }
}