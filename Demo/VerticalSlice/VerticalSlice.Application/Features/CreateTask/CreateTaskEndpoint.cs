using FastEndpoints;
using VerticalSlice.Application.Entities;
using VerticalSlice.Application.Infrastructure.Data;

namespace VerticalSlice.Application.Features.CreateTask;

public class MyEndpoint : Endpoint<CreateTaskRequest, CreateTaskResponse>
{
    private readonly ApplicationDbContext _dbContext;

    public MyEndpoint(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/api/tasks");
        Description(b => b
        .Accepts<CreateTaskRequest>("application/json+custom")
        .Produces<CreateTaskResponse>(200, "application/json+custom")
        .ProducesProblemFE(400)
        .ProducesProblemFE<InternalErrorResponse>(500),
    clearDefaults: true);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateTaskRequest req, CancellationToken ct)
    {

        var entity = new TaskEntity
        {
            Title = req.Title,
            Description = req.Description,
            OwnerId = Guid.NewGuid(),
            Deleted = false
        };

        _dbContext.Tasks.Add(entity);

        await _dbContext.SaveChangesAsync(ct);

        await SendAsync(new CreateTaskResponse(entity.Title, entity.Description));

    }
}
