using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Infrastructure.Email.Smtp;

public record Option([Required(AllowEmptyStrings = false)] string? Host,
[Required(AllowEmptyStrings = false)] int Port,
[Required(AllowEmptyStrings = false)] string? UserName,
[Required(AllowEmptyStrings = false)] string? Password,
[Required(AllowEmptyStrings = false)][EmailAddress] string? FromEmail)
{
    public Option() : this(default, default, default, default, default) { }
}