using System.Net.Mime;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Infrastructure.Auth.Github
{
    public class GithubTokenAuthenticationHandler : AuthenticationHandler<GithubTokenAuthenticationOptions>
    {
        private readonly ILogger<GithubTokenAuthenticationHandler> _logger;
        private readonly GithubTokenAuthenticationOptions _options;
        private readonly IGithubApi _gitHubApi;
        public GithubTokenAuthenticationHandler(IOptionsMonitor<GithubTokenAuthenticationOptions> options,
         ILoggerFactory logger,
         ILogger<GithubTokenAuthenticationHandler> handlerLogger,
         IGithubApi gitHubApi,
         UrlEncoder encoder) : base(options, logger, encoder)
        {
            _logger = handlerLogger;
            _options = options.CurrentValue;
            _gitHubApi = gitHubApi;
        }
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.Headers.Append(HeaderNames.WWWAuthenticate, $@"Authorization realm=""{_options.Scheme}""");
            Response.StatusCode = StatusCodes.Status403Forbidden;
            Response.ContentType = MediaTypeNames.Application.Json;

            var result = new
            {
                Response.StatusCode,
                Message = "Forbidden"
            };

            using var responseStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(responseStream, result);
            await Response.BodyWriter.WriteAsync(responseStream.ToArray());
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!TryGetApiKeyHeader(out string? token, out AuthenticateResult? authenticateResult))
            {
                return authenticateResult!;
            }

            var (isValidated, principal) = await ValidateToken(token!);

            if (isValidated)
            {
                var ticket = new AuthenticationTicket(principal!, _options.Scheme);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.NoResult();
        }

        private async Task<Tuple<bool, ClaimsPrincipal>> ValidateToken(string token)
        {
            var principal = new ClaimsPrincipal();
            try
            {
                var emails = await _gitHubApi.GetEmailAsync(token);


                if (!emails.Any())
                {
                    _logger.LogInformation($"- Github token validation failed\n. - Token ={token}\n - Not found email");

                    return DefaultResult(principal);
                }

                var primaryEmail = emails.Where(x => x.Primary).FirstOrDefault() ?? emails.FirstOrDefault();

                var payload = await _gitHubApi.GetUserAsync(token);

                if (payload is null
                || string.IsNullOrEmpty(payload.Name))
                {
                    _logger.LogInformation($"- Github token validation failed\n. - Token ={token}\n - Not found user");

                    return DefaultResult(principal);
                }

                var claimsIdentity = new ClaimsIdentity(_options.AuthenticationType);

                claimsIdentity.AddClaims(new List<Claim>
                 {
                     new Claim(Global.Claims.NameIdentifier, payload!.Id.ToString()),
                     new Claim(Global.Claims.Name, payload.Name),
                     new Claim(Global.Claims.Email, primaryEmail!.Email),
                 });

                if (!string.IsNullOrEmpty(payload.Avatar_Url))
                {
                    claimsIdentity.AddClaim(
                     new Claim(Global.Claims.AvatarUrl, payload.Avatar_Url));
                }

                principal.AddIdentity(claimsIdentity);

                return Tuple.Create(true, principal);

            }
            catch (Exception ex)
            {
                if (ex is Refit.ApiException)
                {
                    _logger.LogWarning(ex.Message, ex);
                }
                else
                {
                    throw new GithubValidationTokenException(ex.Message, ex);
                }
            }

            return DefaultResult(principal);

            static Tuple<bool, ClaimsPrincipal> DefaultResult(ClaimsPrincipal principal)
            {
                return Tuple.Create(false, principal);
            }
        }
        private bool TryGetApiKeyHeader(out string? apiKeyHeaderValue, out AuthenticateResult? result)
        {
            apiKeyHeaderValue = null;

            if (!Request.Headers.TryGetValue(_options.ApiKey, out var apiKeyHeaderValues))
            {

                result = AuthenticateResult.Fail($"Not found api key {_options.ApiKey}");

                return false;
            }

            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(apiKeyHeaderValues.FirstOrDefault()))
            {
                result = AuthenticateResult.Fail($"The {_options.ApiKey} value is null");

                return false;
            }

            apiKeyHeaderValue = apiKeyHeaderValues.FirstOrDefault();

            result = null;

            return true;
        }
    }
}