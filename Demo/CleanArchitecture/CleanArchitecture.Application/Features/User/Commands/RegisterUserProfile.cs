using System.ComponentModel;
using System.Net;
using AutoMapper;
using FluentValidation;
using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.Core.Abstraction.Message;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Application.Core.Models;
using CleanArchitecture.Domain.Models.Account;
using CleanArchitecture.Domain.Models.Account.DTO;
using CleanArchitecture.Domain.Models.Account.Enum;
using CleanArchitecture.Domain.Models.Account.Validators;

namespace CleanArchitecture.Application.Features.User.Commands;

public static class RegisterUserProfile
{
    [DisplayName("Register User Profile")]
    public sealed class Command : ICommand<OperationResult<UserResponse>>
    {
        public RegisterUserProfileRequest? Request { get; set; }
    }


    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Request).Cascade(CascadeMode.Stop).NotNull().WithMessage("Your request cannot be empty")
            .SetInheritanceValidator(x => x.Add(new RegisterUserProfileRequestValidator()));

        }
    }


    public class Handler : ICommandHandler<Command, OperationResult<UserResponse>>
    {
        private readonly IReadUserService _userService;
        private readonly IGenericService<AccountEntity> _genericService;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public Handler(IReadUserService userService, ICurrentUser currentUser,
        IMapper mapper, IGenericService<AccountEntity> genericService)
        {
            _userService = userService;
            _mapper = mapper;
            _currentUser = currentUser;
            _genericService = genericService;
        }

        public async Task<OperationResult<UserResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var email = _currentUser.GetUserEmail();

            if (string.IsNullOrWhiteSpace(email))
            {
                return OperationResult.Status(HttpStatusCode.Unauthorized);
            }

            var avartarUrlFromToken = _currentUser.GetAvatarUrl();

            var existAccountByEmail = await _userService.GetByEmailAsync(email!, cancellationToken);

            if (existAccountByEmail is not null)
            {
                return await UpdateExistingAccount(request, avartarUrlFromToken, existAccountByEmail, cancellationToken);
            }

            var newAccount = _mapper.Map<AccountEntity>(request.Request);

            newAccount.Email = email;

            if (string.IsNullOrEmpty(newAccount.AvatarUrl))
            {
                newAccount.AvatarUrl = avartarUrlFromToken;
            }

            var insertedAccount = await _genericService.CreateNewAsync(newAccount, cancellationToken);

            return OperationResult.Ok(_mapper.Map<UserResponse>(insertedAccount));

        }

        private async Task<OperationResult<UserResponse>> UpdateExistingAccount(Command request, string? avartarUrlFromToken, AccountEntity existAccountByEmail, CancellationToken cancellationToken)
        {
            var updateAccount = await _genericService.GetByAsync(x => x.Id == existAccountByEmail.Id, true, cancellationToken);

            updateAccount = _mapper.Map(request.Request, updateAccount);

            if (existAccountByEmail.AccountType == AccountType.Profession)
            {
                updateAccount.AccountType = existAccountByEmail.AccountType;
            }

            if (string.IsNullOrEmpty(updateAccount.AvatarUrl))
            {
                updateAccount.AvatarUrl = avartarUrlFromToken;
            }

            await _genericService.UpdateAsync(updateAccount, cancellationToken);

            return OperationResult.Ok(_mapper.Map<UserResponse>(updateAccount));
        }
    }
}
