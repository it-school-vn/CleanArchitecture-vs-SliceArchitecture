using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Application.Features.Event.Commands;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Presentation.Api.Extensions;

namespace CleanArchitecture.Presentation.Api.Features.Event;

public class BookingEventModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {

        const string endPointUrl = "/events/{Id}/booking";

        app.MapPost($"{endPointUrl}", async ([FromRoute] Ulid id, IMediator sender) =>
        {
            var result = await sender.Send(new BookEvent.Command { Id = id });

            return result.ToResult();
        })
       .WithOpenApi()
       .RequireAuthorization(Global.Policy.RegisteredUser);

    }
}