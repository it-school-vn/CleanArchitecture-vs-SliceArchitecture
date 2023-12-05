using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Application.Features.Event.Commands;
using CleanArchitecture.Application.Features.Event.Queries;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Presentation.Api.Extensions;

namespace CleanArchitecture.Presentation.Api.Features.Event;

public class EventModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {

        const string endPointUrl = "/events";

        app.MapPost($"{endPointUrl}", async ([FromBody] CreateEvent.Command command, IMediator sender) =>
        {
            var result = await sender.Send(command);

            return result.ToResult();
        })
       .WithOpenApi()
       .RequireAuthorization(Global.Policy.ProfessionUser);


        app.MapPut($"{endPointUrl}" + "/{id}", async ([FromRoute] Ulid id, [FromBody] UpdateEvent.Command command, IMediator sender) =>
        {
            command.Id = id;
            var result = await sender.Send(command);

            return result.ToResult();
        })
       .WithOpenApi()
       .RequireAuthorization(Global.Policy.ProfessionUser);


        app.MapGet($"{endPointUrl}", async ([AsParameters] ListEvent.Query query, IMediator sender) =>
        {
            var result = await sender.Send(query);

            return result.ToResult();
        })
       .WithOpenApi()
       .RequireAuthorization(Global.Policy.RegisteredUser);

        app.MapGet($"{endPointUrl}" + "/{id}", async ([FromRoute] Ulid id, IMediator sender) =>
            {
                var result = await sender.Send(new GetEvent.Query
                {
                    Id = id
                });

                return result.ToResult();
            })
           .WithOpenApi()
           .RequireAuthorization(Global.Policy.RegisteredUser);
    }
}