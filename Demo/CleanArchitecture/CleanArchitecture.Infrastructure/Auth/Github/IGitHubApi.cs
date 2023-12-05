using Refit;

namespace CleanArchitecture.Infrastructure.Auth.Github
{

    [Headers("Content-Type: application/json", "User-Agent: request")]
    public interface IGithubApi
    {
        [Get("/user")]
        Task<GithubUser?> GetUserAsync([Authorize("Bearer")] string token, CancellationToken cancellationToken = default);

        [Get("/user/emails")]
        Task<IList<GithubUserEmail>> GetEmailAsync([Authorize("Bearer")] string token, CancellationToken cancellationToken = default);
    }

}