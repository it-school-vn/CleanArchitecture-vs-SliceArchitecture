using Microsoft.Extensions.Options;
using CleanArchitecture.Application.Core.Abstraction.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CleanArchitecture.Infrastructure.Email.TwilioSendGrid;
public class SendGridEmailService : IEmailService
{
    private readonly EmailAddress _fromEmail;
    private readonly SendGridClient _client;
    public SendGridEmailService(IOptions<Option> options)
    {
        _fromEmail = new EmailAddress(options.Value.FromEmail);
        _client = new SendGridClient(options.Value.ApiKey);
    }

    public async Task<bool> SendAsync(string toEmail, string subject, string plainTextContent, string htmlContent)
    {
        var msg = MailHelper.CreateSingleEmail(_fromEmail, new EmailAddress(toEmail), subject, plainTextContent, htmlContent);
        var result = await _client.SendEmailAsync(msg).ConfigureAwait(false);

        return result.IsSuccessStatusCode;
    }
}
