using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Infrastructure.Auth;
using CleanArchitecture.Infrastructure.Email;
using CleanArchitecture.Infrastructure.Middleware;
using CleanArchitecture.Infrastructure.Persistence;

namespace CleanArchitecture.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<Encryption.Option>()
        .Bind(configuration.GetSection("Encryption"))
        .ValidateDataAnnotations()
        .ValidateOnStart();

        return services
        .AddEmailService(configuration)
        .AddSingleton<IEncryptionService, Encryption.EncryptionService>()
        .AddMemoryCache()
        .AddDistributedMemoryCache()
        .ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()))
        .AddHttpContextAccessor()
        .AddDbContextAndUnitOfWork(configuration)
        .AddAuth(configuration)
        .AddMiddleware();

    }

}

