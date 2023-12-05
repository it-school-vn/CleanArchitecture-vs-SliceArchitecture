using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using VerticalSlice.Application.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
var configuring = builder.Configuration;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ApplicationDbContext>(options =>
{
    var builder = new DbContextOptionsBuilder();

    builder.EnableSensitiveDataLogging(true);
    builder.LogTo(Console.WriteLine);

    builder.UseNpgsql(configuring["ConnectionStrings"]);

    return new ApplicationDbContext(builder);

});

builder.Services.AddFastEndpoints()
.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "My API";
        s.Version = "v1";
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    using var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    await dbContext!.Database.MigrateAsync();
}

app.UseFastEndpoints()
.UseSwaggerGen();

app.Run();

