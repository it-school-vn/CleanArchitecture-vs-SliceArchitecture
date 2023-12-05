using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using CleanArchitecture.Application.Core.Abstraction.Services;

namespace CleanArchitecture.Infrastructure.Email.Smtp;

public class SmtpEmailService : IEmailService
{
    private readonly Option _option;

    public SmtpEmailService(IOptions<Option> options)
    {
        _option = options.Value;
    }

    public async Task<bool> SendAsync(string toEmail, string subject, string plainTextContent, string htmlContent)
    {
        using var client = new SmtpClient();
        client.Host = _option.Host!;
        client.Port = _option.Port;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential(_option.UserName, _option.Password);
        using var message = new MailMessage(from: new MailAddress(_option.FromEmail!), to: new MailAddress(toEmail));

        message.Subject = subject;
        message.Body = string.IsNullOrEmpty(htmlContent) ? plainTextContent : htmlContent;
        await client.SendMailAsync(message);

        return true;
    }
}