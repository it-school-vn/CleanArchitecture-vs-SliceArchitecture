using Microsoft.Extensions.Caching.Memory;
using CleanArchitecture.Domain.Models.Account;

#nullable disable
namespace CleanArchitecture.Application.BusinessServices.Impl
{
    public class UserReadMemoryCacheService : IReadUserService
    {
        private readonly IReadUserService _userService;
        private readonly IMemoryCache _cache;

        const string CACHE_KEY_PATTERN = $"{nameof(IReadUserService)}_";
        public UserReadMemoryCacheService(IReadUserService userService, IMemoryCache cache)
        {
            _userService = userService;
            _cache = cache;
        }

        public async Task<AccountEntity> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var cacheKey = CACHE_KEY_PATTERN + email;

            if (_cache.TryGetValue<AccountEntity>(cacheKey, out AccountEntity item) && item is not null)
            {
                return item;
            }

            var newItem = await _userService.GetByEmailAsync(email, cancellationToken);

            if (newItem != null)
            {
                _cache.Set(newItem, cacheKey, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                });
            }

            return newItem;

        }
    }
}