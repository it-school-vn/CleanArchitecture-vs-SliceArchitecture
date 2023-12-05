using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using CleanArchitecture.Domain.Constants;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace CleanArchitecture.Infrastructure.Auth.Google
{
    public class GoogleJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {

        private readonly ValidationSettings _validationSettings;
        public GoogleJwtSecurityTokenHandler(ValidationSettings validationSettings)
        {
            InboundClaimTypeMap.Clear();
            _validationSettings = validationSettings;

        }

        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            base.ValidateToken(token, validationParameters, out validatedToken);

            try
            {
                var payload = GoogleJsonWebSignature.ValidateAsync(token, _validationSettings).Result;

                var principal = new ClaimsPrincipal();
                var claimsIdentity = new ClaimsIdentity(GoogleJwtBearerDefaults.AuthenticationType);

                claimsIdentity.AddClaims(new List<Claim>
                 {
                     new Claim(Global.Claims.NameIdentifier, payload.Name),
                     new Claim(Global.Claims.Name, payload.Name),
                     new Claim(Global.Claims.FullName,payload.Name),
                     new Claim(Global.Claims.Email, payload.Email),
                     new Claim(Global.Claims.Iss, payload.Issuer),
                     new Claim(Global.Claims.EmailVerified, payload.EmailVerified.ToString())
                 });

                if (!string.IsNullOrEmpty(payload.FamilyName))
                {
                    claimsIdentity.AddClaim(new Claim(Global.Claims.FamilyName, payload.FamilyName));
                }

                if (!string.IsNullOrEmpty(payload.GivenName))
                {
                    claimsIdentity.AddClaim(new Claim(Global.Claims.GivenName, payload.GivenName));
                }

                if (!string.IsNullOrEmpty(payload.Subject))
                {
                    claimsIdentity.AddClaim(new Claim(Global.Claims.Sub, payload.Subject));
                }

                if (!string.IsNullOrEmpty(payload.Picture))
                {
                    claimsIdentity.AddClaim(new Claim(Global.Claims.AvatarUrl, payload.Picture));
                }

                if (payload.IssuedAtTimeSeconds.HasValue)
                {
                    claimsIdentity.AddClaim(new Claim(Global.Claims.Iat, payload.IssuedAtTimeSeconds.Value.ToString()));
                }



                if (payload.ExpirationTimeSeconds.HasValue)
                {
                    claimsIdentity.AddClaim(new Claim(Global.Claims.ExpiredAt, payload.ExpirationTimeSeconds.Value.ToString()));
                }

                principal.AddIdentity(claimsIdentity);

                return principal;

            }
            catch (Exception e)
            {
                throw new GoogleValidationTokenException(e.Message, e);

            }

        }

    }


}
