using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Core.RequestPipelines;
public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : class
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not null)
        {
            _logger.LogInformation($"Handling {typeof(TRequest).Name}");
            Type myType = request.GetType();

            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            if (props is not null && props.Any())
            {
                foreach (PropertyInfo prop in props)
                {
                    if (prop.GetIndexParameters().Length == 0)
                    {
                        _logger.LogInformation("   {0} ({1}): {2}", prop.Name,
                                          prop.PropertyType.Name,
                                          prop.GetValue(request));
                    }
                    else
                    {
                        _logger.LogInformation("   {0} ({1}): <Indexed>", prop.Name,
                                          prop.PropertyType.Name);
                    }
                }
            }
        }

        var response = await next();

        _logger.LogInformation($"Handled {typeof(TResponse).Name}");

        return response;
    }
}