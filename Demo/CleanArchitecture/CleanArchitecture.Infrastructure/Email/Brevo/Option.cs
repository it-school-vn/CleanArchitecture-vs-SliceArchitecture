using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Infrastructure.Email.Brevo;

public record Option([Required(AllowEmptyStrings = false)] string? ApiUrl,
[Required(AllowEmptyStrings = false)] string? ApiKey,
 [Required(AllowEmptyStrings = false)][EmailAddress] string? FromEmail)
{
    public Option() : this(default, default, default) { }
}