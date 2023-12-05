using Microsoft.AspNetCore.Authentication.JwtBearer;
using static Google.Apis.Auth.GoogleJsonWebSignature;
namespace CleanArchitecture.Infrastructure.Auth.Google;
public static class Startup
{
    public static JwtBearerOptions UseJWTGoogle(this JwtBearerOptions options, string clientId)
    {
        return options.UseJWTGoogle(clientId, new ValidationSettings());
    }

    public static JwtBearerOptions UseJWTGoogle(this JwtBearerOptions options, string clientId, ValidationSettings validateSettings)
    {
        if (clientId == null)
        {
            throw new ArgumentNullException(nameof(clientId));
        }

        if (clientId.Length == 0)
        {
            throw new ArgumentException("ClientId cannot be empty.", nameof(clientId));
        }

        options.TokenHandlers.Clear();
        options.TokenHandlers.Add(new GoogleJwtSecurityTokenHandler(validateSettings));
        options.Audience = clientId;
        options.Authority = GoogleJwtBearerDefaults.Authority;
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.NameClaimType = "name";

        options.TokenValidationParameters.ValidIssuers = new[] { GoogleJwtBearerDefaults.Authority, "accounts.google.com" };

        options.TokenValidationParameters.ValidAudiences = new[]
        {
            clientId
        };
        options.TokenValidationParameters.AuthenticationType = GoogleJwtBearerDefaults.AuthenticationType;
        return options;
    }
}