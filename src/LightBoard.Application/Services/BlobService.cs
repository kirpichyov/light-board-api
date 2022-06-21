using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Options;
using LightBoard.Application.Abstractions.Results;
using LightBoard.Application.Abstractions.Services;
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

    public async Task<UploadBlobResult> UploadFormFile(UploadFormFileArgs arguments)
    {
        var args = new UploadBlobArgs()
        {
            BlobContainer = arguments.Container,
            ContentDisposition = GetNormalizedContentDisposition(arguments.FormFile.FileName, arguments.Purpose),
            ContentType = arguments.FormFile.ContentType,
            ContentStream = arguments.FormFile.OpenReadStream()
        };

        return await UploadBlob(args);
    }

    public async Task<string> GetBlobStringContentAsync(BlobContainer blobContainer, string fileName)
    {
        var containerName = MapToContainerName(blobContainer);
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        MemoryStream memoryStream = new MemoryStream();
        await blobClient.DownloadToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        
        return await new StreamReader(memoryStream).ReadToEndAsync();
    }

    public async Task<UploadBlobResult> UploadStreamContent(UploadFileStreamArgs arguments)
    {
        var args = new UploadBlobArgs()
        {
            BlobContainer = arguments.Container,
            ContentDisposition = GetNormalizedContentDisposition(arguments.FileName, arguments.Purpose),
            ContentType = arguments.ContentType,
            ContentStream = arguments.ContentStream
        };

        return await UploadBlob(args);
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
            BlobContainer.MailTemplates => _blobOptions.ContainerNames.MailTemplates,
            _ => throw new ArgumentException($"Value '{container}' is unexpected.", nameof(container))
        };
    }

    private string GetNormalizedContentDisposition(string fileName, BlobPurpose blobPurpose)
    {
        string type = blobPurpose switch
        {
            BlobPurpose.Inline => "inline",
            BlobPurpose.Attachment => "attachment",
            _ => throw new ArgumentException($"Value '{blobPurpose}' is unexpected.", nameof(blobPurpose))
        };
        
        return $"{type}; filename=\"{fileName.Unidecode()}\";";
    }

    private async Task<UploadBlobResult> UploadBlob(UploadBlobArgs args)
    {
        string containerName = MapToContainerName(args.BlobContainer);
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        
        var blobName = Guid.NewGuid().ToString();
        var blob = containerClient.GetBlobClient(blobName);

        var blobHttpHeader = new BlobHttpHeaders
        {
            ContentType = args.ContentType,
            ContentDisposition = args.ContentDisposition
        };

        await blob.UploadAsync(args.ContentStream, blobHttpHeader);
        var uri = blob.GenerateSasUri(BlobSasPermissions.Read, DefaultSasExpiresOn).ToString();

        return new UploadBlobResult(uri, blobName);
    }
    
    private class UploadBlobArgs
    {
        public BlobContainer BlobContainer { get; init; }
        public string ContentType { get; init; }
        public string ContentDisposition { get; init; }
        public Stream ContentStream { get; init; }
    }
}