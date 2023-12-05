using MediatR;

namespace CleanArchitecture.Application.Core.Abstraction.Message
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}