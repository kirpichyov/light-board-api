using System.ComponentModel.DataAnnotations;

namespace LightBoard.Application.Abstractions.Options;

public class BlobOptions
{
    [Required] public string ConnectionString { get; set; }
    [Required] public BlobContainersNode ContainerNames { get; set; }
}

public class BlobContainersNode
{
    [Required] public string UserAvatars { get; set; }
    [Required] public string BoardBackgrounds { get; set; }
    [Required] public string CardAttachments { get; set; }
}