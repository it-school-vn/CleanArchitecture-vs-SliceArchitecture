using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.BusinessServices.Impl;
using CleanArchitecture.Domain.Models.Account;
using CleanArchitecture.Domain.Models.Account.Enum;
using CleanArchitecture.Domain.Models.Account.ValueObjects;

namespace CleanArchitecture.Application.UnitTest.BusinessServices.Impl;

public class UserReadMemoryCacheServiceTest
{

    internal class MockMemoryCache() : IMemoryCache
    {

        private readonly AccountEntity? _account;
        private readonly bool _hasNullValue;

        public bool HasCallCreateEntry { get; private set; }

        public MockMemoryCache(AccountEntity? mockObject, bool hasNullValue) : this()
        {
            _account = mockObject;
            _hasNullValue = hasNullValue;
        }
        public ICacheEntry CreateEntry(object key)
        {
            HasCallCreateEntry = true;
            return Substitute.For<ICacheEntry>();
        }

        public void Dispose()
        {
        }

        public void Remove(object key)
        {
        }

        public bool TryGetValue(object key, out object? value)
        {
            value = _account;
            return _hasNullValue || _account != null;
        }
    }
    private IMemoryCache? _memoryCache;
    private IReadUserService _userService;
    const string _email = "test@gmail.com";

    private static AccountEntity CreateFakeAccount(string email)
    {
        return new AccountEntity()
        {
            Email = email,
            FirstName = "test",
            LastName = "unit",
            Title = "MrTest",
            JobStatus = Domain.Enums.JobStatus.None,
            DesiredJob = "any",
            Skills = new[] { "net", "java" },
            AccountType = AccountType.Student,
            Student = new StudentInfo("microsost", 2023),
            Profession = new ProfessionInfo("microsoft", 4),
            TimeZone = 7,
            InterviewDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            Point = new PointInfo(0)
        };
    }

    [SetUp]
    public void SetUp()
    {
        _userService = Substitute.For<IReadUserService>();
    }

    [TearDown]
    public void TearDown()
    {
        _memoryCache?.Dispose();
    }

    [Test]
    public async Task GetByEmailAsync_ExistingCacheKeyAndValue_ReturnFromCacheAndNotQuery()
    {
        // Arrange 

        var account = CreateFakeAccount(_email);

        _memoryCache = new MockMemoryCache(account, false);

        var service = new UserReadMemoryCacheService(_userService, _memoryCache);

        // Action

        var getAccount = await service.GetByEmailAsync(_email, default);

        // Assert

        await _userService.DidNotReceive().GetByEmailAsync(_email, Arg.Any<CancellationToken>());

        Assert.That(getAccount, Is.Not.Null);

        Assert.That(getAccount, Is.EqualTo(account));


    }



    [Test]
    public async Task GetByEmailAsync_ExistingCacheKeyAndNullValue_DoQuery()
    {
        // Arrange 

        var account = CreateFakeAccount(_email);

        _memoryCache = new MockMemoryCache(null, true);

        _userService.GetByEmailAsync(_email, Arg.Any<CancellationToken>()).Returns(account);

        var service = new UserReadMemoryCacheService(_userService, _memoryCache);

        // Action

        var getAccount = await service.GetByEmailAsync(_email, default);

        // Assert

        await _userService.Received().GetByEmailAsync(_email, Arg.Any<CancellationToken>());

        Assert.That(getAccount, Is.Not.Null);

        Assert.That(getAccount, Is.EqualTo(account));
    }


    [Test]
    public async Task GetByEmailAsync_NotExistCacheKeyAndNullValue_NotCacheNullValue()
    {

        // Arrange 
        _memoryCache = new MockMemoryCache(null, false);

        _userService.GetByEmailAsync(_email, Arg.Any<CancellationToken>())
        .ReturnsNull<AccountEntity>();

        var service = new UserReadMemoryCacheService(_userService, _memoryCache);

        // Action

        var getAccount = await service.GetByEmailAsync(_email, default);

        // Assert

        await _userService.Received().GetByEmailAsync(_email, Arg.Any<CancellationToken>());

        Assert.IsFalse((_memoryCache as MockMemoryCache)!.HasCallCreateEntry);

        Assert.That(getAccount, Is.Null);
    }

    [Test]
    public async Task GetByEmailAsync_NotExistCacheKey_QueryAndCacheValue()
    {
        // Arrange 

        var account = CreateFakeAccount(_email);

        _memoryCache = new MockMemoryCache(null, false);

        _userService.GetByEmailAsync(_email, Arg.Any<CancellationToken>())
       .Returns(account);

        var service = new UserReadMemoryCacheService(_userService, _memoryCache);

        // Action

        var getAccount = await service.GetByEmailAsync(_email, default);

        // Assert

        await _userService.Received().GetByEmailAsync(_email, Arg.Any<CancellationToken>());

        Assert.That(getAccount, Is.Not.Null);

        Assert.That(getAccount, Is.EqualTo(account));

        Assert.IsTrue((_memoryCache as MockMemoryCache)!.HasCallCreateEntry);
    }
}