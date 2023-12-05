using Carter;
using MediatR;
using CleanArchitecture.Application.Features.FeatureFlag.Queries;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Presentation.Api.Extensions;

namespace CleanArchitecture.Presentation.Api.Features.FeatureFlag;

public class FeatureFlagModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {

        const string endPointUrl = "/feature-flag";

        app.MapGet($"{endPointUrl}", async ([AsParameters] ListFeatureFlag.Query query, IMediator sender) =>
        {
            var result = await sender.Send(query);

            return result.ToResult();
        })
       .WithOpenApi()
       .RequireAuthorization(Global.Policy.RegisteredUser);

    }
}