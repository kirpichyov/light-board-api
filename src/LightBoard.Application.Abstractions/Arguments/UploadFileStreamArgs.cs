namespace LightBoard.Application.Abstractions.Arguments;

public class UploadFileStreamArgs
{
    public BlobContainer Container { get; init; }
    public BlobPurpose Purpose { get; init; }
    public Stream ContentStream { get; init; }
    public string FileName { get; init; }
    public string ContentType { get; init; }
}