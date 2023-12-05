global using CleanArchitecture.Domain.Entities;
global using CleanArchitecture.Domain.Enums;
global using CleanArchitecture.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.BusinessServices.Impl;
using CleanArchitecture.Application.Core.RequestPipelines;
using AutoMapper.EquivalencyExpression;
using CleanArchitecture.Application.Core.Configurations;
using Microsoft.Extensions.Configuration;

namespace CleanArchitecture.Application
{
    public static class Startup
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddOptions<Hosting>()
                    .Bind(configuration.GetSection("Hosting"))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddCollectionMappers();
            }, typeof(Startup), typeof(Domain.Startup));

            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);
            services.AddMediatR(cfg =>
                       {
                           cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly);
                       });

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddScoped<IReadUserService, UserReadService>();
            services.Decorate<IReadUserService, UserReadMemoryCacheService>();
            services.AddSingleton<ITemplateService, TemplateService>();

            return services;
        }
    }
}