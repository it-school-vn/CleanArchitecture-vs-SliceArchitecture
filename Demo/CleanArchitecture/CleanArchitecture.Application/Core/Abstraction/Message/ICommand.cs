using MediatR;

namespace CleanArchitecture.Application.Core.Abstraction.Message
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}