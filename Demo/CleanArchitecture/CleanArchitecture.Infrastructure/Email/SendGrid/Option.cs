using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Infrastructure.Email.TwilioSendGrid;

public record Option([Required(AllowEmptyStrings = false)] string? ApiKey, [Required(AllowEmptyStrings = false)][EmailAddress] string? FromEmail)
{
    public Option() : this(default, default) { }
}
