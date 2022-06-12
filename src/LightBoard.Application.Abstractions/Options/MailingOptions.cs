using System.ComponentModel.DataAnnotations;

namespace LightBoard.Application.Abstractions.Options;

public class MailingOptions
{
    [Required] public string Email { get; set; }
    [Required] public string Host { get; set; }
    [Required] public int Port { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string FromName { get; set; }
}