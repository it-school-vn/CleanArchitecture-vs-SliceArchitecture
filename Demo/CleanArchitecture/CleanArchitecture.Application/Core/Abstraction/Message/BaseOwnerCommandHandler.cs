
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Application.Core.CustomExceptions;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Core.Abstraction.Message;

#nullable disable
public abstract class BaseOwnerCommandHandler<TId, TCommand, TResponse> : ICommandHandler<TCommand, TResponse> where TCommand : IOwnerCommand<TId, TResponse>
{
    protected readonly ICurrentUser _currentUser;

    protected BaseOwnerCommandHandler(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    protected virtual Task<IOwner> GetItemOwner(TCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(default(IOwner));
    }

    protected virtual Task<TResponse> HandleMain(TCommand request,
    IOwner owner,
    CancellationToken cancellationToken)
    {
        return Task.FromResult(default(TResponse))!;
    }
    public async Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken)
    {

        var owner = await GetItemOwner(request, cancellationToken);

        if (owner is null || owner.OwnerId != _currentUser.GetUserId())
        {
            throw new NotFoundException($"Not found the item with id ={request.Id}");
        }

        return await HandleMain(request!, owner, cancellationToken);
    }
}