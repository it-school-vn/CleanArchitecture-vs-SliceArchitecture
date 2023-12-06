using AutoMapper;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Application.Features.User.Commands;
using CleanArchitecture.Domain.Models.Account;
using CleanArchitecture.Domain.Models.Account.Enum;
using CleanArchitecture.Domain.Models.Account.ValueObjects;

namespace CleanArchitecture.Application.UnitTest.Features.User.Commands
{
    public class RegisterUserTest
    {
        private RegisterUserProfile.Handler _handler;
        private IMapper _mapper;

        private IReadUserService _userService;

        private IGenericService<AccountEntity> _genericService;

        private ICurrentUser _currentUser;

        [SetUp]
        public void SetUp()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            //mapperConfiguration.AssertConfigurationIsValid();
            _mapper = mapperConfiguration.CreateMapper();

            _userService = Substitute.For<IReadUserService>();


            _currentUser = Substitute.For<ICurrentUser>();

            _genericService = Substitute.For<IGenericService<AccountEntity>>();


            _handler = new RegisterUserProfile.Handler(_userService, _currentUser, _mapper, _genericService);

        }

        private static RegisterUserProfile.Command CreateValidCommand() => new RegisterUserProfile.Command
        {
            Request = new Domain.Models.Account.DTO.RegisterUserProfileRequest
            {
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
                InterviewDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1))
            }
        };

        [Test]
        public async Task Handler_ExistingEmail_UpdateAccountProfile()
        {
            // Arrange
            var command = CreateValidCommand();
            const string email = "unit@gmail.com";

            var account = _mapper.Map<AccountEntity>(command.Request);

            account.Email = email;

            _currentUser.GetUserEmail().Returns(x => email);

            _userService.GetByEmailAsync(email, default).Returns(account);

            // Action
            var result = await _handler.Handle(command, default);

            // Assert

            Assert.IsTrue(result.Succeeded);

        }

        [TestCase(AccountType.Student)]
        [TestCase(AccountType.Profession)]
        public async Task Handler_ValidCommand_ReturnSuceededResult(AccountType accountType)
        {
            // Arrange

            var command = CreateValidCommand();

            command.Request!.AccountType = accountType;

            const string email = "unit@gmail.com";

            var account = _mapper.Map<AccountEntity>(command.Request);

            account.Email = email;

            _currentUser.GetUserEmail().Returns(x => email);

            _userService.GetByEmailAsync(email).ReturnsNull();

            _genericService.CreateNewAsync(Arg.Any<AccountEntity>(), default).Returns(account);

            // Action
            var result = await _handler.Handle(command, default);

            // Assert

            Assert.True(result.Succeeded);

            Assert.That(result.Value.Email, Is.EqualTo(email));

            Assert.That(result.Value.AccountType, Is.EqualTo(accountType));

        }
        #region  Validation
        [TestCase("FirstName", null)]
        [TestCase("FirstName", "")]
        [TestCase("FirstName", "     ")]
        [TestCase("LastName", null)]
        [TestCase("LastName", "")]
        [TestCase("LastName", "     ")]
        [TestCase("Title", null)]
        [TestCase("Title", "")]
        [TestCase("Title", "     ")]
        [TestCase("DesiredJob", null)]
        [TestCase("DesiredJob", "")]
        [TestCase("DesiredJob", "     ")]
        public void Validation_MissingRequiredFields_ReturnErrors(string propertyName, object? value)
        {
            // Arrange
            var command = CreateValidCommand();


            Helper.SetPropertyValue(propertyName, command.Request!, value);


            var validator = new RegisterUserProfile.Validator();

            IDictionary<string, string> message = new Dictionary<string, string>() {
                {
                "FirstName","Your first name cannot be empty"
                },
                {
                "LastName","Your last name cannot be empty"
                },
                {
                "Title","Your title cannot be empty"
                },
                {
                "DesiredJob","Your desired job cannot be empty"
                }

                };

            var requestProperty = $"Request.{propertyName}";
            //Action
            var result = validator.Validate(command);

            //Assert

            Assert.That(result.IsValid, Is.False);

            Assert.That(result.Errors.Any(y => y.PropertyName == requestProperty), Is.True);

            Assert.That(result.Errors.Any(y => y.ErrorMessage ==
             message[propertyName]), Is.True);


        }

        [TestCase(-14)]
        [TestCase(16)]
        public void Validation_InvalidTimeZone_ReturnErrors(int timeZone)
        {
            // Arrange
            const string propertyName = "TimeZone";
            var command = CreateValidCommand();

            Helper.SetPropertyValue("TimeZone", command.Request!, timeZone);

            var validator = new RegisterUserProfile.Validator();

            //Action
            var result = validator.Validate(command);

            //Assert

            Assert.That(result.IsValid, Is.False);

            Assert.That(result.Errors.Any(y => y.PropertyName == $"Request.{propertyName}"), Is.True);

        }
        #endregion
    }
}