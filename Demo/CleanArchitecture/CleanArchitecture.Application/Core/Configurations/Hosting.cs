using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Application.Core.Configurations;
public record Hosting([Required] string? Domain, [Required] string? AbsoluteUri)
{
    public Hosting() : this(default, default) { }
}