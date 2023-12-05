using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Application.Core.CustomExceptions;
using CleanArchitecture.Infrastructure.Email.Brevo;
using UseBrevo = CleanArchitecture.Infrastructure.Email.Brevo;
using UseSendGrid = CleanArchitecture.Infrastructure.Email.TwilioSendGrid;
using UseSmtp = CleanArchitecture.Infrastructure.Email.Smtp;
namespace CleanArchitecture.Infrastructure.Email;

public static class Startup
{
    private static IServiceCollection RegisterSendGridEmailService(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddOptions<UseSendGrid.Option>()
       .Bind(configuration.GetSection("EmailService:SendGrid"))
       .ValidateDataAnnotations()
       .ValidateOnStart();

        services.AddSingleton<IEmailService, UseSendGrid.SendGridEmailService>();

        return services;

    }

    private static IServiceCollection RegisterBrevoEmailService(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddOptions<UseBrevo.Option>()
        .Bind(configuration.GetSection("EmailService:Brevo"))
        .ValidateDataAnnotations()
        .ValidateOnStart();

        const string apiUrlKey = "Service:Brevo:ApiUrl";

        var apiUrl = configuration[apiUrlKey];

        NullConfigurationException.ThrowIfNullOrEmpty(apiUrl, apiUrlKey);

        services
            .AddRefitClient<IBreroEmailApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiUrl));

        services.AddSingleton<IEmailService, UseBrevo.BrevoEmailService>();
        return services;

    }

    private static IServiceCollection RegisterSmtpEmailService(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddOptions<UseSmtp.Option>()
        .Bind(configuration.GetSection("EmailService:Smtp"))
        .ValidateDataAnnotations()
        .ValidateOnStart();

        services.AddSingleton<IEmailService, UseSmtp.SmtpEmailService>();

        return services;

    }


    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        const string emailProviderKey = "EmailService:Provider";
        var emailProviderConfig = configuration[emailProviderKey];

        NullConfigurationException.ThrowIfNullOrWhiteSpace(emailProviderConfig, emailProviderKey);

        var emailProvider = Enum.Parse<EmailProvider>(emailProviderConfig);

        return emailProvider switch
        {
            EmailProvider.UseSendGrid => services.RegisterSendGridEmailService(configuration),
            EmailProvider.UseSmtp => services.RegisterSmtpEmailService(configuration),
            EmailProvider.UseBrevo => services.RegisterBrevoEmailService(configuration),
            _ => throw new DependencyException(nameof(IEmailService))
        };

    }



}