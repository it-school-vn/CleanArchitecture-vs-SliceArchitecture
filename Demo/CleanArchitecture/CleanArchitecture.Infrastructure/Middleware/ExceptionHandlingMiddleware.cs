using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Application.Core.CustomExceptions;

namespace CleanArchitecture.Infrastructure.Middleware
{
    public sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                await HandleExceptionAsync(context, e);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);

            var response = new
            {
                Title = GetTitle(exception),
                Status = statusCode,
                Detail = exception.Message,
                Errors = GetErrors(exception),

            };

            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                ForbiddenException => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

        private static string GetTitle(Exception exception) =>
            exception switch
            {
                CustomApplicationException applicationException => applicationException.Title,
                _ => "Server Error"
            };

        private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
        {

            if (exception is ValidationException validationException)
            {
                return validationException.Errors
                .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                })
                .ToDictionary(x => x.Key, x => x.Values); ;
            }

            if (exception is BadRequestException badRequestException)
            {
                return badRequestException.Errors;
            }

            return new Dictionary<string, string[]>();
        }
    }
}