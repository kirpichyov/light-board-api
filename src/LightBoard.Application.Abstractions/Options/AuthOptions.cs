using System.ComponentModel.DataAnnotations;

namespace LightBoard.Application.Abstractions.Options;

public class AuthOptions
{
    [Required] public int SessionDaysLifetime { get; set; }
    [Required] public int SessionKeyLength { get; set; }
    [Required] public int CodeMinutesLifetime { get; set; }
    [Required] public string[]? AllowedCorsList { get; set; }
}