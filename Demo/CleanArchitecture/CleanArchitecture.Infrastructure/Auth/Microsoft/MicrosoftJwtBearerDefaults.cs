using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CleanArchitecture.Infrastructure.Auth.Microsoft
{
    public static class MicrosoftJwtBearerDefaults
    {
        /// <summary>
        /// The default authority for Google authentication.
        /// </summary>
        public static readonly string Authority = "https://login.microsoftonline.com/common/v2.0";

        /// <summary>
        /// The default authentication scheme for Google authentication.
        /// </summary>
        public static readonly string AuthenticationScheme = "Microsoft." + JwtBearerDefaults.AuthenticationScheme;

        public static readonly string AuthenticationType = AuthenticationScheme;
    }
}