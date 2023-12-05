using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure.Middleware;

public static class Startup
{

    public static IServiceCollection AddMiddleware(this IServiceCollection services)
    {
        services.AddScoped<ExceptionHandlingMiddleware>();
        return services;
    }
}
