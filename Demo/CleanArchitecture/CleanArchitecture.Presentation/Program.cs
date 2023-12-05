using Carter;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Application.Core.CustomExceptions;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Middleware;
using CleanArchitecture.Infrastructure.Auth;
using CleanArchitecture.Presentation.Api;
using Microsoft.AspNetCore.Http.Json;
using CleanArchitecture.Infrastructure.Converters;

var cancellationToken = new CancellationTokenSource();

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new Cysharp.Serialization.Json.UlidJsonConverter());
    options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());

})
.AddCustomSwagger()
.AddInfrastructure(configuration)
.AddApplication(configuration)
.AddCarter()
.AddCors();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    using var migrateService = scope.ServiceProvider.GetService<IDataMigrationService>() ?? throw new DependencyException(nameof(IDataMigrationService));
    await migrateService.MigrateAsync(cancellationToken.Token);
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseCurrentUser();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapCarter();
app.Run();

