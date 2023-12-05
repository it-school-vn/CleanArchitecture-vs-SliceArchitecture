using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Application.Features.User.Commands;
using CleanArchitecture.Application.Features.User.Queries;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Presentation.Api.Extensions;

namespace CleanArchitecture.Application.Features.User;
public class UserProfileModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("users/login-profile", async (IMediator sender) =>
        {
            var result = await sender.Send(new GetLoginProfile.Query());
            return result.ToResult();
        })
        .WithOpenApi()
        .RequireAuthorization(Global.Policy.AuthenticatedUser);

        app.MapPost($"users/register-profile", async ([FromBody] RegisterUserProfile.Command command, IMediator sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        })
        .WithOpenApi()
        .RequireAuthorization(Global.Policy.AuthenticatedUser);

    }

}