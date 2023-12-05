
namespace CleanArchitecture.Infrastructure.Email.Brevo;

public record BrevoEmail(EmailAddress From, EmailAddress To, string Subject, string HtmlContent);

public record EmailAddress(string Email, string Name);
