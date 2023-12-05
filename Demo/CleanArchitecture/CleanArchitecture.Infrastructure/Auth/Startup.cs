using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Models.Account.Enum;
using CleanArchitecture.Infrastructure.Auth.Github;
using CleanArchitecture.Infrastructure.Auth.Google;
using CleanArchitecture.Infrastructure.Auth.Microsoft;

namespace CleanArchitecture.Infrastructure.Auth;

public static class Startup
{
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        services
        .AddTransient<IClaimsTransformation, MyClaimsTransformation>()
        .AddGitHubAuth(config)
        .AddAuthentication(x => x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(GoogleJwtBearerDefaults.AuthenticationScheme, option =>
                {
                    option.UseJWTGoogle(config["Authentication:Google:ClientId"]!);

                })
                .AddJwtBearer(MicrosoftJwtBearerDefaults.AuthenticationScheme, option =>
                {
                    option.UseJwtMicrosoft(config["Authentication:Microsoft:ClientId"]!);
                })
                .AddGitHubAuth(options => { });

        var allSchemes = new[] { JwtBearerDefaults.AuthenticationScheme,
         GoogleJwtBearerDefaults.AuthenticationScheme,
         MicrosoftJwtBearerDefaults.AuthenticationScheme,
         GithubTokenDefaults.AuthenticationScheme};

        services.AddAuthorization();
        services.AddAuthorizationBuilder()
            .AddPolicy(Global.Policy.AuthenticatedUser, policy =>
                policy.RequireAuthenticatedUser()
                .RequireClaim(ClaimTypes.Email)
                .AuthenticationSchemes = allSchemes
                )
            .AddPolicy(Global.Policy.Admin, policy =>
                policy
                    .RequireAuthenticatedUser()
                    .RequireClaim(Global.Claims.UserId)
                    .RequireRole(Global.AccountRole.Admin)
                    .RequireClaim(Global.Claims.Scope, Global.ApiScope.Admin)
                    .AuthenticationSchemes = allSchemes
                    )
            .AddPolicy(Global.Policy.RegisteredUser, policy =>
                 policy
                    .RequireAuthenticatedUser()
                    .RequireRole(Global.AccountRole.User, Global.AccountRole.User, Global.AccountRole.Admin)
                    .RequireClaim(Global.Claims.Scope, Global.ApiScope.User)
                    .RequireClaim(Global.Claims.UserId)
                    .AuthenticationSchemes = allSchemes
                    )
            .AddPolicy(Global.Policy.ProfessionUser, policy =>
                 policy
                    .RequireAuthenticatedUser()
                    .RequireRole(Global.AccountRole.User)
                    .RequireClaim(Global.Claims.UserId)
                    .RequireClaim(Global.Claims.Scope, Global.ApiScope.User)
                    .RequireClaim(Global.Claims.AccountType, AccountType.Profession.ToString())
                    .AuthenticationSchemes = allSchemes
                    )
            .AddPolicy(Global.Policy.StudentUser, policy =>
                 policy
                    .RequireAuthenticatedUser()
                    .RequireRole(Global.AccountRole.User)
                    .RequireClaim(Global.Claims.UserId)
                    .RequireClaim(Global.Claims.Scope, Global.ApiScope.User)
                    .RequireClaim(Global.Claims.AccountType, AccountType.Student.ToString())
                    .AuthenticationSchemes = allSchemes
                    );

        return services
                 .AddCurrentUser();
    }


    public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
       app.UseMiddleware<CurrentUserMiddleware>();

    private static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
        services
            .AddScoped<CurrentUserMiddleware>()
            .AddScoped<ICurrentUser, CurrentUser>()
            .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());
}
