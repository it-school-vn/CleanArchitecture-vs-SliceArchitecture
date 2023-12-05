namespace CleanArchitecture.Application.Core.Abstraction.Message
{
    public interface IIdempotentCommand<out TResponse> : ICommand<TResponse>
    {
        Guid RequestId { get; set; }
    }
}