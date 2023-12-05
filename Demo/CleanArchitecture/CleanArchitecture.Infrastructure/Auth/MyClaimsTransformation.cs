using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.Core.Extensions;
using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Application.Core.Abstraction.Services
{
    public class MyClaimsTransformation : IClaimsTransformation
    {
        private readonly IReadUserService _userService;

        public MyClaimsTransformation(IReadUserService userService)
        {
            _userService = userService;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var userEmail = principal.GetEmail();

            if (!string.IsNullOrEmpty(userEmail))
            {
                ClaimsIdentity claimsIdentity = new ClaimsIdentity();

                var accountEntity = await _userService
                .GetByEmailAsync(userEmail);

                if (accountEntity is not null)
                {
                    if (!principal.HasClaim(claim => claim.Type == Global.Claims.UserId))
                    {
                        claimsIdentity
                        .AddClaim(new Claim(
                            Global.Claims.UserId,
                            accountEntity.Id.ToString()
                         ));

                    }

                    if (!principal.HasClaim(claim => claim.Type == Global.Claims.AccountType))
                    {
                        var accountTypeName = Enum.GetName(accountEntity.AccountType);

                        if (!string.IsNullOrEmpty(accountTypeName))
                        {
                            claimsIdentity
                            .AddClaim(new Claim(
                                Global.Claims.AccountType,
                                accountTypeName
                             ));
                        }
                    }

                    if (!principal.HasClaim(claim => claim.Type == Global.Claims.AccountStatus))
                    {
                        var status = Enum.GetName(accountEntity.Status);

                        if (!string.IsNullOrEmpty(status))
                        {
                            claimsIdentity
                            .AddClaim(new Claim(
                                Global.Claims.AccountStatus,
                                status
                             ));
                        }
                    }


                    if (!principal.HasClaim(claim => claim.Type == Global.Claims.Role))
                    {

                        if (!string.IsNullOrEmpty(accountEntity.Role))
                        {
                            claimsIdentity
                            .AddClaim(new Claim(
                                Global.Claims.Role,
                                accountEntity.Role
                             ));
                        }
                    }


                    if (!principal.HasClaim(claim => claim.Type == Global.Claims.Scope))
                    {
                        var scope = accountEntity.Role switch
                        {
                            Global.AccountRole.User => Global.ApiScope.User,
                            Global.AccountRole.Admin => Global.ApiScope.Admin,
                            _ => Global.ApiScope.None

                        };

                        claimsIdentity
                            .AddClaim(new Claim(
                                Global.Claims.Scope,
                                scope
                             ));
                    }

                    if (!principal.HasClaim(claim => claim.Type == Global.Claims.AvatarUrl) || !string.IsNullOrEmpty(accountEntity.AvatarUrl))
                    {
                        claimsIdentity
                            .AddClaim(new Claim(
                                Global.Claims.AvatarUrl,
                                accountEntity.AvatarUrl!
                             ));
                    }

                    principal.AddIdentity(claimsIdentity);
                }
            }

            return principal;
        }
    }
}