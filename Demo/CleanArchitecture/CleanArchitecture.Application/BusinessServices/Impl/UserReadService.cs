using CleanArchitecture.Domain.Models.Account;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Application.BusinessServices.Impl
{
    public class UserReadService : IReadUserService
    {
        private readonly IUnitOfWork _unit;
        public UserReadService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<AccountEntity> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _unit.GetReadRepository<AccountEntity>().FirstOrDefaultAsync(x => x.Email.ToUpper() == email.ToUpper(), cancellationToken);
        }

    }
}