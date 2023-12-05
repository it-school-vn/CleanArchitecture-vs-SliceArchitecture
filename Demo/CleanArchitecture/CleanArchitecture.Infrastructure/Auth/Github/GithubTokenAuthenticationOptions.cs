using Microsoft.AspNetCore.Authentication;

namespace CleanArchitecture.Infrastructure.Auth.Github
{
    public class GithubTokenAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Scheme { get; set; } = GithubTokenDefaults.AuthenticationScheme;
        public string AuthenticationType { get; set; } = GithubTokenDefaults.AuthenticationScheme;

        public string ApiKey { get; set; } = GithubTokenDefaults.ApiKey;
    }
}