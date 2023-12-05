namespace VerticleSlice.Application.Features.CreateToDo;

public class CreateToDoEndpoint::EndpointBaseAsync
    .WithRequest<CreateToDoRequest>
    .WithResult<ActionResult<CreateToDoResponse>>
{

  [HttpPost("api/todo/")]
public override async Task<ActionResult<string[]>> HandleAsync(
    CreateToDoRequest request,
    CancellationToken cancellationToken = default)
{
    /// put your business logic here
    /// Validation by using FluenValidation
    /// Saving entity to DB via Efcore or Dapper
}
}

