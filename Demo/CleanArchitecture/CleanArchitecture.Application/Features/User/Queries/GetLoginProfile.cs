using System.Net;
using AutoMapper;
using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.Core.Abstraction.Message;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Application.Core.Models;
using CleanArchitecture.Domain.Models.Account.DTO;

namespace CleanArchitecture.Application.Features.User.Queries
{
    public static class GetLoginProfile
    {
        public class Query : IQuery<OperationResult<UserResponse>>
        {

        }
        public class Handler : IQueryHandler<Query, OperationResult<UserResponse>>
        {
            private readonly ICurrentUser _currentUser;
            private readonly IReadUserService _userService;

            private readonly IMapper _mapper;

            public Handler(ICurrentUser currentUser,
             IReadUserService userService, IMapper mapper)
            {
                _currentUser = currentUser;
                _userService = userService;
                _mapper = mapper;
            }

            public async Task<OperationResult<UserResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var email = _currentUser.GetUserEmail();

                var account = await _userService.GetByEmailAsync(email!, cancellationToken);

                if (account is null)
                {
                    return OperationResult.Status(HttpStatusCode.NotFound);
                }

                var result = _mapper.Map<UserResponse>(account);

                return OperationResult.Ok(result);
            }
        }

    }
}