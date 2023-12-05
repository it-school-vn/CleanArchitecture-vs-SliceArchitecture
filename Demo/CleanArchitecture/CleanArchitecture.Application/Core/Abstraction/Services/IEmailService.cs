namespace CleanArchitecture.Application.Core.Abstraction.Services;
public interface IEmailService
{
    Task<bool> SendAsync(string toEmail, string subject, string plainTextContent, string htmlContent);
}