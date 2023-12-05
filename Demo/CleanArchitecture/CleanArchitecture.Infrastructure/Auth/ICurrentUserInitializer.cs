using System.Security.Claims;

namespace CleanArchitecture.Infrastructure.Auth;

public interface ICurrentUserInitializer
{
    void SetCurrentUser(ClaimsPrincipal user);

    void SetCurrentUserId(string userId);
}
