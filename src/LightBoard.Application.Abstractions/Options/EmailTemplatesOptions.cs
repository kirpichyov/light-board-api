using System.ComponentModel.DataAnnotations;

namespace LightBoard.Application.Abstractions.Options;

public class EmailTemplatesOptions
{
    [Required] public string EmailConfirmationTemplateFilename { get; set; }
    [Required] public string PasswordResetTemplateFilename { get; set; }
}