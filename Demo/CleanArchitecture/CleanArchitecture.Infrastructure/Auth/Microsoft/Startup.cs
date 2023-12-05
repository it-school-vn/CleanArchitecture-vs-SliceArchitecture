
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CleanArchitecture.Infrastructure.Auth.Microsoft;
public static class Startup
{

    public static JwtBearerOptions UseJwtMicrosoft(this JwtBearerOptions options, string clientId)
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
        options.TokenHandlers.Add(new JwtSecurityTokenHandler());
        options.Audience = clientId;
        options.Authority = MicrosoftJwtBearerDefaults.Authority;
        options.TokenValidationParameters.ValidateIssuer = false;
        options.TokenValidationParameters.NameClaimType = "name";

        options.TokenValidationParameters.ValidIssuers = new[] { MicrosoftJwtBearerDefaults.Authority, "login.microsoftonline.com/common/v2.0" };

        options.TokenValidationParameters.ValidAudiences = new[]
        {
            clientId
        };
        options.TokenValidationParameters.AuthenticationType = MicrosoftJwtBearerDefaults.AuthenticationType; ;
        return options;
    }
}