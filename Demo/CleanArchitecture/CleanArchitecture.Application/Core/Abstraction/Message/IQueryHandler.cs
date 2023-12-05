using MediatR;

namespace CleanArchitecture.Application.Core.Abstraction.Message
{
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
           where TQuery : IQuery<TResponse>
    {
    }
}