
using Microsoft.Extensions.Options;
using CleanArchitecture.Application.Core.Abstraction.Services;

namespace CleanArchitecture.Infrastructure.Email.Brevo;

public class BrevoEmailService : IEmailService
{
    private readonly Option _option;
    private readonly IBreroEmailApi _emailApi;

    public BrevoEmailService(IOptions<Option> options, IBreroEmailApi emailApi)
    {
        _option = options.Value;
        _emailApi = emailApi;
    }

    public async Task<bool> SendAsync(string toEmail, string subject, string plainTextContent, string htmlContent)
    {
        var brevoEmail = new BrevoEmail(new EmailAddress(_option.FromEmail!, _option.FromEmail!),
         new EmailAddress(toEmail, toEmail),
         subject,
         string.IsNullOrEmpty(htmlContent) ? plainTextContent : htmlContent);

        var response = await _emailApi.SendAsync(_option.ApiKey!, brevoEmail);

        return response.IsSuccessStatusCode;

    }
}