using CleanArchitecture.Domain.Constants;
using FluentValidation;

namespace CleanArchitecture.Domain.Validators;

public class UrlValidator : AbstractValidator<string>
{
    public UrlValidator()
    {
        RuleFor(x => x).Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _) && uri.Length <= Global.UrlLength)
                 .When(x => !string.IsNullOrEmpty(x))
                 .WithMessage($"Your url must be a uri and lenght must not exceed {Global.UrlLength} ");
    }
}