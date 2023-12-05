using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using CleanArchitecture.Application.Core.CustomExceptions;
namespace CleanArchitecture.Infrastructure.Auth.Github;
public static class Startup
{

    public static IServiceCollection AddGitHubAuth(this IServiceCollection services, IConfiguration configuration)
    {
        const string gitHubApiSettingKey = "Authentication:Github:ApiUrl";

        var gitHubApiUrl = configuration[gitHubApiSettingKey];

        if (string.IsNullOrWhiteSpace(gitHubApiUrl))
        {
            throw new NullConfigurationException(gitHubApiSettingKey);
        }

        services
        .AddRefitClient<IGithubApi>()
        .ConfigureHttpClient(c => c.BaseAddress = new Uri(gitHubApiUrl));

        return services;
    }

    public static AuthenticationBuilder AddGitHubAuth(this AuthenticationBuilder authenticationBuilder, Action<GithubTokenAuthenticationOptions> options)
            => authenticationBuilder.AddScheme<GithubTokenAuthenticationOptions, GithubTokenAuthenticationHandler>(GithubTokenDefaults.AuthenticationScheme, options);
}