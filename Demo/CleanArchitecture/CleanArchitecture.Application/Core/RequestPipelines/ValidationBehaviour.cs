using FluentValidation;
using MediatR;
using CleanArchitecture.Application.Core.Abstraction.Message;

namespace CleanArchitecture.Application.Core.RequestPipelines
{
    public sealed class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, ICommand<TResponse>, IQuery<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var errorsDictionary = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();
            if (errorsDictionary.Any())
            {
                throw new ValidationException(errorsDictionary);
            }

            return await next();
        }


    }
}