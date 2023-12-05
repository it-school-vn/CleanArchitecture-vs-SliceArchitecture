using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CleanArchitecture.Infrastructure.Auth.Google
{
    public static class GoogleJwtBearerDefaults
    {
        /// <summary>
        /// The default authority for Google authentication.
        /// </summary>
        public static readonly string Authority = "https://accounts.google.com";

        /// <summary>
        /// The default authentication scheme for Google authentication.
        /// </summary>
        public static readonly string AuthenticationScheme = "Google." + JwtBearerDefaults.AuthenticationScheme;

        public static readonly string AuthenticationType = AuthenticationScheme;

    }
}