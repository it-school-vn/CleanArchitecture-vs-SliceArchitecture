using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Infrastructure.Encryption;

public record Option
{
    [Required]
    public required string Key { get; init; }

    [Required]
    public required string Salt { get; init; }
}