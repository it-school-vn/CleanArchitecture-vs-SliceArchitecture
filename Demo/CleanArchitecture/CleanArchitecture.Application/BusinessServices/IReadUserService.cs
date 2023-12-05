using CleanArchitecture.Domain.Models.Account;

namespace CleanArchitecture.Application.BusinessServices
{
    public interface IReadUserService
    {
        Task<AccountEntity> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}