using System.Security.Claims;
using CleanArchitecture.Domain.Models.Account.Enum;

namespace CleanArchitecture.Application.Core.Abstraction.Services;

public interface ICurrentUser
{
    string? Name { get; }

    Guid GetUserId();

    string? GetUserEmail();

    string? GetAvatarUrl();

    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();

    string? GetScope();

    IEnumerable<string>? GetRoles();

    AccountType GetAccountType { get; }

    AccountStatus Status { get; }

}
