using System.Security.Claims;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Models.Account.Enum;

namespace CleanArchitecture.Application.Core.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal principal)
       => principal.FindFirstValue(Global.Claims.Email);

    public static string? GetFullName(this ClaimsPrincipal principal)
        => principal?.FindFirst(Global.Claims.FullName)?.Value;

    public static string? GetFirstName(this ClaimsPrincipal principal)
        => principal?.FindFirst(Global.Claims.GivenName)?.Value;

    public static string? GetSurname(this ClaimsPrincipal principal)
        => principal?.FindFirst(Global.Claims.FamilyName)?.Value;

    public static string? GetPhoneNumber(this ClaimsPrincipal principal)
        => principal.FindFirstValue(Global.Claims.MobilePhone);

    public static string? GetUserId(this ClaimsPrincipal principal)
       => principal.FindFirstValue(Global.Claims.UserId);

    public static string? GetAvatarUrl(this ClaimsPrincipal principal)
       => principal.FindFirstValue(Global.Claims.AvatarUrl);

    public static string? GetScope(this ClaimsPrincipal principal)
       => principal.FindFirstValue(Global.Claims.Scope);

    public static IEnumerable<string> GetRoles(this ClaimsPrincipal principal)
        => principal.FindAll(c => c.Type == Global.Claims.Role).Select(x => x.Value).ToArray();

    public static AccountType? GetAccountType(this ClaimsPrincipal principal)
    {
        var item = principal?.FindFirstValue(Global.Claims.AccountType);
        if (item is not null && Enum.TryParse(item, out AccountType accountType))
        {
            return accountType;
        }

        return null;
    }
    public static AccountStatus? GetAccountStatus(this ClaimsPrincipal principal)
    {
        var item = principal?.FindFirstValue(Global.Claims.AccountStatus);
        if (item is not null && Enum.TryParse(item, out AccountStatus status))
        {
            return status;
        }

        return null;
    }


    private static string? FindFirstValue(this ClaimsPrincipal principal, string claimType) =>
        principal is null
            ? throw new ArgumentNullException(nameof(principal))
            : principal.FindFirst(claimType)?.Value;
}
