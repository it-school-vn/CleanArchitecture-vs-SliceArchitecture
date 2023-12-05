using System.Security.Claims;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Application.Core.Extensions;
using CleanArchitecture.Domain.Models.Account.Enum;
namespace CleanArchitecture.Infrastructure.Auth
{
    public class CurrentUser : ICurrentUser, ICurrentUserInitializer
    {
        private ClaimsPrincipal? _user;

        public string? Name => _user?.Identity?.Name;

        public AccountType GetAccountType => IsAuthenticated()
        ? _user?.GetAccountType() ?? AccountType.None : AccountType.None;

        public AccountStatus Status => IsAuthenticated() ? _user?.GetAccountStatus() ??
        AccountStatus.Registered : AccountStatus.Registered;

        private Guid _userId = Guid.Empty;

        public Guid GetUserId() =>
            IsAuthenticated()
                ? Guid.Parse(_user?.GetUserId() ?? Guid.Empty.ToString())
                : _userId;

        public string? GetUserEmail() =>
            IsAuthenticated()
                ? _user!.GetEmail()
                : string.Empty;

        public bool IsAuthenticated() =>
            _user?.Identity?.IsAuthenticated is true;

        public bool IsInRole(string role) =>
            _user?.IsInRole(role) is true;

        public IEnumerable<Claim>? GetUserClaims() =>
            _user?.Claims;

        public void SetCurrentUser(ClaimsPrincipal user)
        {
            if (_user != null)
            {
                throw new Exception("Method reserved for in-scope initialization");
            }

            _user = user;
        }

        public void SetCurrentUserId(string userId)
        {
            if (_userId != Guid.Empty)
            {
                throw new Exception("Method reserved for in-scope initialization");
            }

            if (!string.IsNullOrEmpty(userId))
            {
                _userId = Guid.Parse(userId);
            }
        }

        public string? GetScope() =>
            IsAuthenticated()
                ? _user!.GetScope()
                : string.Empty;


        IEnumerable<string>? ICurrentUser.GetRoles() =>
            IsAuthenticated()
                ? _user!.GetRoles()
                : Enumerable.Empty<string>();

        public string? GetAvatarUrl() =>
            IsAuthenticated()
                ? _user!.GetAvatarUrl()
                : string.Empty;

    }

}